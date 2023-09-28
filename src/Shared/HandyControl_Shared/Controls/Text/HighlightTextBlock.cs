using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace HandyControl.Controls;

public class HighlightTextBlock : TextBlock
{
    public static readonly DependencyProperty SourceTextProperty =
        DependencyProperty.Register(nameof(SourceText), typeof(string), typeof(HighlightTextBlock),
            new PropertyMetadata(null, OnSourceTextChanged));

    public static readonly DependencyProperty QueriesTextProperty =
        DependencyProperty.Register(nameof(QueriesText), typeof(string), typeof(HighlightTextBlock),
            new PropertyMetadata(null, OnQueriesTextChanged));

    public static readonly DependencyProperty HighlightBrushProperty =
        DependencyProperty.Register(nameof(HighlightBrush), typeof(Brush), typeof(HighlightTextBlock));

    public static readonly DependencyProperty HighlightTextBrushProperty =
        DependencyProperty.Register(nameof(HighlightTextBrush), typeof(Brush), typeof(HighlightTextBlock));

    private static void OnSourceTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        ((HighlightTextBlock) d).RefreshInlines();

    private static void OnQueriesTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        ((HighlightTextBlock) d).RefreshInlines();

    /// <summary>
    /// Replace the original property Text for binding text.
    /// </summary>
    /// <remarks>
    /// Don't use the <see cref="TextBlock.Text"/> property!
    /// Because the <see cref="TextBlock.Text"/> has some unique behaviors
    /// that is not needed at <see cref="HighlightTextBlock"/> at <see cref="TextBlock"/>,
    /// which will cause some unexpected issue. 
    /// </remarks>
    public string SourceText
    {
        get => (string) GetValue(SourceTextProperty);
        set => SetValue(SourceTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the text that need to be highlighted.
    /// It can be an array of text separated by spaces.
    /// </summary>
    public string QueriesText
    {
        get => (string) GetValue(QueriesTextProperty);
        set => SetValue(QueriesTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="Brush"/> of the background of the highlight text.
    /// </summary>
    public Brush HighlightBrush
    {
        get => (Brush) GetValue(HighlightBrushProperty);
        set => SetValue(HighlightBrushProperty, value);
    }

    public Brush HighlightTextBrush
    {
        get => (Brush) GetValue(HighlightTextBrushProperty);
        set => SetValue(HighlightTextBrushProperty, value);
    }

    private void RefreshInlines()
    {
        Inlines.Clear();

        if (string.IsNullOrEmpty(SourceText)) return;
        if (string.IsNullOrEmpty(QueriesText))
        {
            Inlines.Add(SourceText);
            return;
        }

        var sourceText = SourceText;
        var queries = QueriesText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var intervals = from query in queries.Distinct()
                        from interval in GetQueryIntervals(sourceText, query)
                        select interval;
        var mergedIntervals = MergeIntervals(intervals.ToList());
        var fragments = SplitTextByOrderedDisjointIntervals(sourceText, mergedIntervals);

        Inlines.AddRange(GenerateRunElement(fragments));
    }

    private IEnumerable GenerateRunElement(IEnumerable<Fragment> fragments)
    {
        return from item in fragments
               select item.IsQuery
                   ? GetHighlightRun(item.Text)
                   : new Run(item.Text);
    }

    private Run GetHighlightRun(string highlightText)
    {
        var run = new Run(highlightText);

        run.SetBinding(TextElement.BackgroundProperty, new Binding(nameof(HighlightBrush)) { Source = this });
        run.SetBinding(TextElement.ForegroundProperty, new Binding(nameof(HighlightTextBrush)) { Source = this });

        return run;
    }

    private static IEnumerable<Fragment> SplitTextByOrderedDisjointIntervals(string sourceText, List<Range> mergedIntervals)
    {
        if (string.IsNullOrEmpty(sourceText)) yield break;

        if (!mergedIntervals?.Any() ?? true)
        {
            yield return new Fragment { Text = sourceText, IsQuery = false };
            yield break;
        }

        var range0 = mergedIntervals.First();
        int start0 = range0.Start;
        int end0 = range0.End;

        if (start0 > 0) yield return new Fragment { Text = sourceText.Substring(0, start0), IsQuery = false };
        yield return new Fragment { Text = sourceText.Substring(start0, end0 - start0), IsQuery = true };

        int previousEnd = end0;
        foreach (var range in mergedIntervals.Skip(1))
        {
            int start = range.Start;
            int end = range.End;
            yield return new Fragment { Text = sourceText.Substring(previousEnd, start - previousEnd), IsQuery = false };
            yield return new Fragment { Text = sourceText.Substring(start, end - start), IsQuery = true };
            previousEnd = end;
        }

        if (previousEnd < sourceText.Length)
            yield return new Fragment { Text = sourceText.Substring(previousEnd), IsQuery = false };
    }

    private static List<Range> MergeIntervals(List<Range> intervals)
    {
        if (!intervals?.Any() ?? true) return new List<Range>();

        intervals.Sort((x, y) => x.Start != y.Start ? x.Start - y.Start : x.End - y.End);

        var pointer = intervals[0];
        int startPointer = pointer.Start;
        int endPointer = pointer.End;

        var result = new List<Range>();
        foreach (var range in intervals.Skip(1))
        {
            int start = range.Start;
            int end = range.End;

            if (start <= endPointer)
            {
                if (endPointer < end)
                {
                    endPointer = end;
                }
            }
            else
            {
                result.Add(new Range { Start = startPointer, End = endPointer });
                startPointer = start;
                endPointer = end;
            }
        }
        result.Add(new Range { Start = startPointer, End = endPointer });
        return result;
    }

    private static IEnumerable<Range> GetQueryIntervals(string sourceText, string query)
    {
        if (string.IsNullOrEmpty(sourceText) || string.IsNullOrEmpty(query)) yield break;

        int nextStartIndex = 0;
        while (nextStartIndex < sourceText.Length)
        {
            int index = sourceText.IndexOf(query, nextStartIndex, StringComparison.CurrentCultureIgnoreCase);

            if (index == -1) yield break;

            nextStartIndex = index + query.Length;
            yield return new Range { Start = index, End = nextStartIndex };
        }
    }

    private struct Fragment
    {
        public string Text { get; set; }

        public bool IsQuery { get; set; }
    }

    private struct Range
    {
        public int Start { get; set; }

        public int End { get; set; }
    }
}

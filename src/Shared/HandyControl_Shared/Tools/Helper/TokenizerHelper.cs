//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
// File: TokenizerHelper.cs
//
// Description: This file contains the implementation of TokenizerHelper.
//              This class should be used by most - if not all - MIL parsers.
//
// History:
//  05/19/2003 : Microsoft - Created it
//  05/20/2003 : Microsoft - Moved to Shared
//
//---------------------------------------------------------------------------

using System;
using System.Globalization;

namespace HandyControl.Tools;

internal class TokenizerHelper
{
    private char _quoteChar;

    private char _argSeparator;

    private string _str;

    private int _strLen;

    private int _charIndex;

    private int _currentTokenIndex;

    private int _currentTokenLength;

    public bool FoundSeparator { get; private set; }

    public TokenizerHelper(string str, IFormatProvider formatProvider)
    {
        var numberSeparator = GetNumericListSeparator(formatProvider);
        Initialize(str, '\'', numberSeparator);
    }

    private void Initialize(string str, char quoteChar, char separator)
    {
        _str = str;
        _strLen = str?.Length ?? 0;
        _currentTokenIndex = -1;
        _quoteChar = quoteChar;
        _argSeparator = separator;

        // immediately forward past any whitespace so
        // NextToken() logic always starts on the first
        // character of the next token.
        while (_charIndex < _strLen)
        {
            if (!char.IsWhiteSpace(_str, _charIndex))
            {
                break;
            }

            ++_charIndex;
        }
    }

    public string GetCurrentToken() =>
        _currentTokenIndex < 0 ? null : _str.Substring(_currentTokenIndex, _currentTokenLength);

    internal bool NextToken() => NextToken(false);

    public bool NextToken(bool allowQuotedToken) => NextToken(allowQuotedToken, _argSeparator);

    public bool NextToken(bool allowQuotedToken, char separator)
    {
        _currentTokenIndex = -1; // reset the currentTokenIndex
        FoundSeparator = false; // reset

        // If we're at end of the string, just return false.
        if (_charIndex >= _strLen)
        {
            return false;
        }

        var currentChar = _str[_charIndex];

        // setup the quoteCount
        var quoteCount = 0;

        // If we are allowing a quoted token and this token begins with a quote,
        // set up the quote count and skip the initial quote
        if (allowQuotedToken &&
            currentChar == _quoteChar)
        {
            quoteCount++; // increment quote count
            ++_charIndex; // move to next character
        }

        var newTokenIndex = _charIndex;
        var newTokenLength = 0;

        // loop until hit end of string or hit a , or whitespace
        // if at end of string ust return false.
        while (_charIndex < _strLen)
        {
            currentChar = _str[_charIndex];

            // if have a QuoteCount and this is a quote
            // decrement the quoteCount
            if (quoteCount > 0)
            {
                // if anything but a quoteChar we move on
                if (currentChar == _quoteChar)
                {
                    --quoteCount;

                    // if at zero which it always should for now
                    // break out of the loop
                    if (0 == quoteCount)
                    {
                        ++_charIndex; // move past the quote
                        break;
                    }
                }
            }
            else if (char.IsWhiteSpace(currentChar) || currentChar == separator)
            {
                if (currentChar == separator)
                {
                    FoundSeparator = true;
                }
                break;
            }

            ++_charIndex;
            ++newTokenLength;
        }

        // if quoteCount isn't zero we hit the end of the string
        // before the ending quote
        if (quoteCount > 0)
        {
            throw new InvalidOperationException("TokenizerHelperMissingEndQuote");
        }

        ScanToNextToken(separator); // move so at the start of the nextToken for next call

        // finally made it, update the _currentToken values
        _currentTokenIndex = newTokenIndex;
        _currentTokenLength = newTokenLength;

        if (_currentTokenLength < 1)
        {
            throw new InvalidOperationException("TokenizerHelperEmptyToken");
        }

        return true;
    }

    private void ScanToNextToken(char separator)
    {
        // if already at end of the string don't bother
        if (_charIndex >= _strLen) return;

        var currentChar = _str[_charIndex];

        // check that the currentChar is a space or the separator.  If not
        // we have an error. this can happen in the quote case
        // that the char after the quotes string isn't a char.
        if (currentChar != separator && !char.IsWhiteSpace(currentChar))
        {
            throw new InvalidOperationException("TokenizerHelperExtraDataEncountered");
        }

        // loop until hit a character that isn't
        // an argument separator or whitespace.
        // !!!Todo: if more than one argSet throw an exception
        var argSepCount = 0;
        while (_charIndex < _strLen)
        {
            currentChar = _str[_charIndex];

            if (currentChar == separator)
            {
                FoundSeparator = true;
                ++argSepCount;
                _charIndex++;

                if (argSepCount > 1)
                {
                    throw new InvalidOperationException("TokenizerHelperEmptyToken");
                }
            }
            else if (char.IsWhiteSpace(currentChar))
            {
                ++_charIndex;
            }
            else
            {
                break;
            }
        }

        // if there was a separatorChar then we shouldn't be
        // at the end of string or means there was a separator
        // but there isn't an arg

        if (argSepCount > 0 && _charIndex >= _strLen)
        {
            throw new InvalidOperationException("TokenizerHelperEmptyToken");
        }
    }

    internal static char GetNumericListSeparator(IFormatProvider provider)
    {
        var numericSeparator = ',';

        // Get the NumberFormatInfo out of the provider, if possible
        // If the IFormatProvider doesn't not contain a NumberFormatInfo, then
        // this method returns the current culture's NumberFormatInfo.
        var numberFormat = NumberFormatInfo.GetInstance(provider);

        // Is the decimal separator is the same as the list separator?
        // If so, we use the ";".
        if (numberFormat.NumberDecimalSeparator.Length > 0 && numericSeparator == numberFormat.NumberDecimalSeparator[0])
        {
            numericSeparator = ';';
        }

        return numericSeparator;
    }
}

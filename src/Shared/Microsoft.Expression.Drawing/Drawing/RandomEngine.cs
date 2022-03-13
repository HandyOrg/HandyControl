using System;

namespace HandyControl.Expression.Drawing;

internal class RandomEngine
{
    private double? _anotherSample;

    private Random _random;

    public RandomEngine(long seed) => Initialize(seed);

    private double Gaussian()
    {
        double num;
        double num2;
        double num3;
        if (_anotherSample.HasValue)
        {
            var num4 = _anotherSample.Value;
            _anotherSample = null;
            return num4;
        }

        do
        {
            num2 = 2.0 * Uniform() - 1.0;
            num3 = 2.0 * Uniform() - 1.0;
            num = num2 * num2 + num3 * num3;
        } while (num >= 1.0);

        var num5 = Math.Sqrt(-2.0 * Math.Log(num) / num);
        _anotherSample = num2 * num5;
        return num3 * num5;
    }

    private void Initialize(long seed) => _random = new Random((int) seed);

    public double NextGaussian(double mean, double variance) => Gaussian() * variance + mean;

    public double NextUniform(double min, double max) => Uniform() * (max - min) + min;

    private double Uniform() => _random.NextDouble();
}

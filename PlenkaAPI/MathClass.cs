﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Math;
namespace PlenkaAPI
{
    public struct CalculationParameters
    {
        public double W;
        public double H;
        public double L;
        public double p;
        public double c;
        public double T0;
        public double Vu;
        public double Tu;
        public double u0;
        public double b;
        public double Tr;
        public double n;
        public double au;
        public double step;
    }

    public struct CalculationResults
    {
        public Dictionary<double, double> Ti { get; set; }
        public Dictionary<double, double> Ni { get; set; }
        public double Q;
        public double T;
        public double N;
    }
    public class MathClass // todo как-то красиво переписать все это
    {
        public MathClass(CalculationParameters cp)
        {
            this.cp = cp;
        }
        #region Parameters

        private CalculationParameters cp;
        private double W => cp.W;
        private double H => cp.H;
        private double L => cp.L;
        private double p => cp.p;
        private double c => cp.c;
        private double T0 => cp.T0;
        private double Vu => cp.Vu;
        private double Tu => cp.Tu;
        private double u0 => cp.u0;
        private double b => cp.b;
        private double Tr => cp.Tr;
        private double n => cp.n;
        private double au => cp.au;
        private double step => cp.step;

        #endregion
        public CalculationResults calculate()
        {
            var F = 0.125 * Pow(cp.H / cp.W, 2);
            var gamma = cp.Vu / cp.H;
            var qGamma = H * W * u0 * Pow(gamma, n + 1);
            var qAlpha = W * au * (1 / b - Tu + Tr);
            var Qch = ((H * W * Vu) / 2) * F;
            var Ti = new Dictionary<double,double>();
            var Ni = new Dictionary<double, double>();
            for (double i = 0; i <= L; i+=step)
            {
                Ti.Add(i,Tr + (1 / b) * Log((b * qGamma + W * au) /
                    (b * qAlpha) * (1 - Exp(-((i * b * qAlpha) /
                    (p * c * Qch)))) + Exp(b * (T0 - Tr - (i * qAlpha) / (p * c * Qch)))));
                Ni.Add(i,u0 * Exp(-b * (Ti[i] - Tr)) * Pow(gamma, n - 1));
            }

            var Q = p * Qch * 3600;
            var T = Ti.Last().Value;
            var N = Ni.Last().Value;
            return new CalculationResults(){Q=Q,T=T,N=N,Ti=Ti,Ni=Ni};
        }

    }
}
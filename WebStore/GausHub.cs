using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore
{
    public class GausHub : Hub
    {

        public async Task Send(string name, string message)
        {
            await Clients.All.SendAsync("Send",name,message);
        }


        public async Task Main(int n, string name)
        {
            double[,] mat = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        mat[i, j] = i + 1;
                    }
                    else
                    {
                        mat[i, j] = i + 2;
                    }
                }
            }
            double[] free = new double[n];
            for (int i = 1; i <= n; i++)
            {
                free[i - 1] = 7;
            }
            await Gauss(name, mat, free, n);
        }

        public async Task<double[]> Gauss(string name,double[,] mat, double[] free, int n)
        {
            var progressStep = 100 / n;
            var progres = 0;
            double[] x = new double[n];
            for (int i = 0; i < n; i++)
            {
                double val = mat[i, i];
                for (int j = i; j < n; j++)
                {
                    double koef = mat[j, i] / val;
                    for (int q = i; q < n; q++)
                    {
                        if (j != i)
                        {
                            mat[j, q] -= mat[i, q] * koef;
                        }
                    }
                }
                progres += progressStep;
                await Send(name, progres.ToString());
            }

            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = free[i];
                for (int j = 0; j < i; j++)
                {
                    free[j] -= x[i] * mat[j, i];
                }
            }
            await Send(name,"100");
            return x;
        }
    }
}

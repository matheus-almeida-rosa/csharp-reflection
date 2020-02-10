﻿using CsharpReflection.Infrastructure;
using CsharpReflection.Services;
using System.IO;
using System.Reflection;

namespace CsharpReflection.Controllers
{
    public class ExchangeController : BaseController
    {
        private IExchangeService _exchangeService;

        public ExchangeController()
        {
            _exchangeService = new ExchangeService();
        }

        public string MXN()
        {
            var finalValue = _exchangeService.GetCurrentExchange("MXN", "BRL", 1);

            var content = View();
            var resultText = content.Replace("{{value}}", finalValue.ToString());

            return resultText;
        }

        public string USD()
        {
            var finalValue = _exchangeService.GetCurrentExchange("USD", "BRL", 1);

            var content = View();
            var resultText = content.Replace("{{value}}", finalValue.ToString());

            return resultText;
        }

        public string Calc(string originCurrency, string targetCurrency, double value)
        {
            var finalValue = _exchangeService.GetCurrentExchange(originCurrency, targetCurrency, value);

            var content = View();
            var resultText = content
                .Replace("{{valueOrigin}}", finalValue.ToString())
                .Replace("{{origin}}", originCurrency)
                .Replace("{{target}}", targetCurrency)
                .Replace("{{valueTarget}}", value.ToString());

            return resultText;
        }

        public string Calc(string originCurrency, double value) => Calc("BRL", originCurrency, value);
    }
}

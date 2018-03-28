﻿using System;

namespace BudgetStuffTests
{
    public class Period
    {
        public Period(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new InvalidException();

            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public int Days()
        {
            return (EndDate - StartDate).Days + 1;
        }

        public int EffectiveDays(Budget budget, Period period)
        {
            var startDate = StartDate < period.StartDate
                ? period.StartDate
                : StartDate;


            var endDate = EndDate > period.EndDate
                ? period.EndDate
                : EndDate;

            return (int) (endDate.AddDays(1) - startDate).TotalDays;
        }
    }
}
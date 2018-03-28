﻿using System;
using System.Collections.Generic;

namespace BudgetStuffTests
{
    public class BudgetManager
    {
        private readonly IRepository<Budget> _repo;

        public BudgetManager(IRepository<Budget> repo)
        {
            _repo = repo;
        }

        public decimal TotalAmount(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new InvalidException();

            var budgetMap = _repo.GetBudget(startDate, endDate);
            if (IsMultipleBudgets(budgetMap))
            {
                decimal amount = 0;
                int index = 0;
                foreach (var month in budgetMap.Keys)
                {
                    int timeSpan = 0;
                    if (IsFirstMonth(index))
                    {
                        timeSpan = DateTime.DaysInMonth(month.Year, month.Month) - startDate.Day + 1;
                    }
                    else if (IsLastMonth(index, budgetMap))
                    {
                        timeSpan = endDate.Day;
                    }
                    else
                    {
                        timeSpan = DateTime.DaysInMonth(month.Year, month.Month);
                    }
                    amount += GetAmount(DateTime.DaysInMonth(month.Year, month.Month), budgetMap[month].amount,
                        timeSpan);
                    index++;
                }
                return amount;
            }
            var timeSpan2 = (endDate - startDate).Days + 1;
            return GetAmount(DateTime.DaysInMonth(startDate.Year, startDate.Month), budgetMap[startDate].amount,
                timeSpan2);
        }

        private static bool IsLastMonth(int index, Dictionary<DateTime, Budget> budgetMap)
        {
            return index == budgetMap.Keys.Count - 1;
        }

        private static bool IsFirstMonth(int index)
        {
            return index == 0;
        }

        private static bool IsMultipleBudgets(Dictionary<DateTime, Budget> budgetMap)
        {
            return budgetMap.Keys.Count > 1;
        }

        private static decimal GetAmount(int monthdays, int amount, int actualdays)
        {
            return amount / monthdays * actualdays;
            //return BudgetMap[startdate].amount / DateTime.DaysInMonth(startdate.Year, startdate.Month) * (timeSpan.Days + 1);
        }

        // private decimal GetBudget
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;

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
            var period = new Period(startDate, endDate);

            var budgets = _repo.GetBudgets();
            var budgetMap = budgets.ToDictionary(x => x.FirstDay, x => x);
            if (OnlyOneBudget(budgets))
            {
                return EffectiveAmount(period, budgets[0]);
            }
            else
            {
                decimal totalAmount = 0;
                int index = 0;
                foreach (var month in budgetMap.Keys)
                {
                    var effectiveDays = EffectiveDays(startDate, endDate, index, month, budgetMap);

                    totalAmount += EffectiveAmount(DateTime.DaysInMonth(month.Year, month.Month),
                        budgetMap[month].Amount,
                        effectiveDays);
                    index++;
                }
                return totalAmount;
            }
        }

        private static int EffectiveDays(DateTime startDate, DateTime endDate, int index, DateTime month,
            Dictionary<DateTime, Budget> budgetMap)
        {
            int effectiveDays = 0;
            if (IsFirstMonth(index))
            {
                effectiveDays = DateTime.DaysInMonth(month.Year, month.Month) - startDate.Day + 1;
            }
            else if (IsLastMonth(index, budgetMap))
            {
                effectiveDays = endDate.Day;
            }
            else
            {
                effectiveDays = DateTime.DaysInMonth(month.Year, month.Month);
            }
            return effectiveDays;
        }

        private static decimal EffectiveAmount(Period period, Budget budget)
        {
            var effectiveDays = period.Days();

            return budget.DailyAmount() * effectiveDays;
        }

        private static bool OnlyOneBudget(List<Budget> budgets)
        {
            return budgets.Count == 1;
        }

        private static decimal EffectiveAmount(int daysOfBudgetMonth, int amountOfBudget, int effectiveDays)
        {
            return amountOfBudget / daysOfBudgetMonth * effectiveDays;
        }

        private static bool IsFirstMonth(int index)
        {
            return index == 0;
        }

        private static bool IsLastMonth(int index, Dictionary<DateTime, Budget> budgetMap)
        {
            return index == budgetMap.Keys.Count - 1;
        }

        // private decimal GetBudget
    }
}
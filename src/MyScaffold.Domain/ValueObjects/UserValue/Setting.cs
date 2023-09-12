﻿using MyScaffold.Domain.Entities;

namespace MyScaffold.Domain.ValueObjects.UserValue
{
    public class Setting
    {
        public string Language { get; set; } = "Chinese";

        #region navigation

        public User User { get; set; } = null!;

        #endregion navigation
    }
}
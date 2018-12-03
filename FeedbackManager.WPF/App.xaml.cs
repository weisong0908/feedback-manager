﻿using AutoMapper;
using FeedbackManager.WPF.Mapping;
using FeedbackManager.WPF.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FeedbackManager.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string ConnectionString { get; } = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\weisong.teng\Desktop\Feedback Database - V1.3.mdb;Persist Security Info=False;";

        public IFeedbackService FeedbackService;

        protected override void OnStartup(StartupEventArgs e)
        {
            FeedbackService = new LocalFeedbackService();
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());

            var startupWindow = new Views.FeedbackRegisterWindow();
            startupWindow.Show();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWStatsUWPLib;
using SpotifyAPI.Web;
using Windows.ApplicationModel.DataTransfer;

using static UWStatsUWPLib.SettingsProvider;

namespace UWStats
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Auth.isAuthorized)
            {
                this.Frame.Navigate(typeof(InterfacePage), AccessToken);
            }
            else
            {
                await ContentDialog1.ShowAsync();
                DataPackage pkg = new DataPackage();
                SettingsProvider.ClientID = ContentDialog1.Tag.ToString();
                (string verifier, string challenge) = Auth.VerifierAndChallenge;
                var loginRequest = new LoginRequest(
                  new Uri("http://localhost:5543/callback"),
                  SettingsProvider.ClientID,
                  LoginRequest.ResponseType.Code
                )
                {
                    CodeChallengeMethod = "S256",
                    CodeChallenge = challenge,
                    Scope = new[] { Scopes.UserTopRead, Scopes.UserReadEmail, Scopes.UserReadPrivate }
                };
                var uri = loginRequest.ToUri();
                this.Frame.Navigate(typeof(AuthWebPage), uri);
            }
        }
    }
}

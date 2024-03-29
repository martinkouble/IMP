﻿using IMP_reseni.Views;
using Microsoft.Extensions.DependencyInjection;
using IMP_reseni.Services;
using IMP_reseni.MyPermissions;

namespace IMP_reseni;

public partial class App : Application
{
    public static SaveHolder saveholder { get; set; }
    //public static CloudService cloudService { get; set; }
    public static BasketHolder basketHolder { get; set; }

    //private string Passwd;
    public App(SaveHolder sp,BasketHolder bs)
    {
        //SecureStorage.Default.RemoveAll();

        saveholder = sp;
        basketHolder = bs;

        //cloudService = cs;
        //SecureStorage.Default.RemoveAll();

        sp.Load();
        if (!sp.ExistSupplierByName("Vlastní výroba"))
        {
            Supplier supp = new Supplier();
            supp.Name = "Vlastní výroba";
            sp.AddSupplier(supp);
        }
        Task get = new Task(GetPassword);
        get.Start();
       InitializeComponent();

        get.Wait();

        //InitializeComponent(); //TEST


        //Task<string> oauthToken = SecureStorage.Default.GetAsync("oauth_token").Wait();

        //MainPage = new ContentPage(new MainPage());

        //MainPage = new AppShell();
    }
    //private async void SetPassword() 
    //{
    //    await SecureStorage.SetAsync("token", "SUS");
    //}

    private async void GetPassword()
    {
        string Passwd = await SecureStorage.Default.GetAsync("token");
        if (Passwd != null)
        {
            MainPage = new NavigationPage(new MainPage());
        }
        else
        {
            MainPage = new NavigationPage(new SetPassword());
        }
    }
    


}

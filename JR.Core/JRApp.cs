using JR.Core.ApplicationObjects;
using JR.Core.Interfaces;
using JR.DL.Model.Entities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.ViewModels;
using JR.SL.Service.Interfaces;
using JR.SL.Service.Impl;

namespace JR.Core
{
    public abstract class BaseJRApp 
        : MvxApplication        
    {
        protected BaseJRApp()
        {
            InitialisePlugins();
            InitialiseText();
            InitialiseServices();
            InitaliseErrorSystem();
        }

        private void InitialisePlugins()
        {
			Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();

            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.JsonLocalisation.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.ResourceLoader.PluginLoader.Instance.EnsureLoaded();
			Cirrious.MvvmCross.Plugins.Messenger.PluginLoader.Instance.EnsureLoaded();

            // these don't really need to be loaded on startup, but it's convenient for now
            Cirrious.MvvmCross.Plugins.Email.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.PhoneCall.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.Share.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.WebBrowser.PluginLoader.Instance.EnsureLoaded();
        }

        private void InitaliseErrorSystem()
        {
            var errorHub = new ErrorApplicationObject();
            Mvx.RegisterSingleton<IErrorReporter>(errorHub);
            Mvx.RegisterSingleton<IErrorSource>(errorHub);
        }

        private void InitialiseServices()
        {
            var repository = new JRService();
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Mvx.RegisterSingleton<IJRService>(repository);
        }

        private void InitialiseText()
        {
            var builder = new TextProviderBuilder();
            // TODO - could choose a language here: builder.LoadResources(whichLanguage);
            Mvx.RegisterSingleton<IMvxTextProvider>(builder.TextProvider);
        }

        protected abstract void InitialiseStartNavigation();
    }

    public class JRApp
        : BaseJRApp
    {
        public JRApp()
        {
            InitialiseStartNavigation();
        }

        protected sealed override void InitialiseStartNavigation()
        {
            var startApplicationObject = new AppStart(true);
            Mvx.RegisterSingleton<IMvxAppStart>(startApplicationObject);
        }
    }

    public class NoSplashScreenJRApp
        : BaseJRApp
    {
        public NoSplashScreenJRApp()
        {
            InitialiseStartNavigation();
        }

        protected sealed override void InitialiseStartNavigation()
        {
            var startApplicationObject = new AppStart(false);
            Mvx.RegisterSingleton<IMvxAppStart>(startApplicationObject);
        }
    }
}

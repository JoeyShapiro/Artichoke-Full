using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

#if __ANDROID__
[assembly: XmlnsDefinition("PLATFORM_OS", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific;assembly=Xamarin.Forms.Core")]
#elif __IOS__
[assembly: XmlnsDefinition("PLATFORM_OS", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core")]
#endif
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Runtime.InteropServices;

namespace TravelEase.Infrastructure.Persistence.Services.PDFServices
{

    public static class PdfConverterFactory
    {
        public static IConverter CreateConverter()
        {
            string libraryPath;
            string dllFolder = Path.Combine(AppContext.BaseDirectory, "DinkToPdf", "libwkhtmltox");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                libraryPath = Path.Combine(dllFolder, "libwkhtmltox.dll");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                libraryPath = Path.Combine(dllFolder, "libwkhtmltox.so");
            else
                throw new PlatformNotSupportedException("Only Windows and Linux are supported");

            libraryPath = Path.GetFullPath(libraryPath);

            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(libraryPath);


            return new SynchronizedConverter(new PdfTools());
        }
    }
}
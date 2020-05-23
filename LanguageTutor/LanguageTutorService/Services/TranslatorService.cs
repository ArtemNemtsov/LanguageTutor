using LanguageTutor.Pages;
using LanguageTutorService.Models;
using System.Threading.Tasks;
using YandexTranslateCSharpSdk;

namespace LanguageTutorService
{
    public class TranslatorService
    {
        private readonly YandexTranslateSdk wrapper; 
        public TranslatorService()
        {
            wrapper = new YandexTranslateSdk
            {
                ApiKey = "trnsl.1.1.20200519T112827Z.1053654c7e28d437.2fdfff42068a541be05f65834133be92e152599f"
            };
        }

        public async Task<string> TranslateAsync(string text, Languages lngFrom, Languages lngTo)
        {
            return await wrapper.TranslateText(text, $"{lngFrom.ToString()}-{lngTo.ToString()}");
        }

        public async Task<string> TranslateAsync(string text, InputLangOption langOption)
        {
            return await wrapper.TranslateText(text, $"{langOption.LngFrom.ToString()}-{langOption.LngTo.ToString()}");
        }
    }
}

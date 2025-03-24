using WebApplication1.Core.Entities;

namespace WebApplication1.Api.DTOs
{
    public class AddProductTranslationRequestDto
    {
        public LanguageCode LanguageCode { get; set; } // e.g., "en", "fr", "ar"
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

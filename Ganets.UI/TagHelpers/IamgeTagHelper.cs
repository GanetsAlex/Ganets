using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

[HtmlTargetElement("img", Attributes = "img-action, img-controller")]
public class ImageTagHelper : TagHelper
{
    private readonly LinkGenerator _linkGenerator;

    // Конструктор с внедрением LinkGenerator
    public ImageTagHelper(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    // Атрибуты тэг-хелпера
    public string ImgController { get; set; }
    public string ImgAction { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Формируем значение атрибута src
        string srcValue = _linkGenerator.GetPathByAction(ImgAction, ImgController);

        // Устанавливаем значение src
        output.Attributes.SetAttribute("src", srcValue);
    }
}

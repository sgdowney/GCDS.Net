using System.Linq.Expressions;

namespace Gcds.Net.Properties
{
    internal class ResourceFormat
    {
        public static string FormatPropertyOfTypeCannotBeNull(string expressionProperty, string expression)
        {
            return string.Format(Resources.PropertyOfTypeCannotBeNull, expressionProperty, expression);
        }

        public static string FormatExpressionHelper_InvalidIndexerExpression(Expression indexExpression, string paramName)
        {
            return string.Format(Resources.ExpressionHelper_InvalidIndexerExpression, indexExpression, paramName);
        }

        public static string FormatHtmlGenerator_FieldNameCannotBeNullOrEmpty(string? typeFullName, string editorName, string? helperFullName, string editorFullName, string fieldName)
        {
            return string.Format(Resources.HtmlGenerator_FieldNameCannotBeNullOrEmpty, typeFullName, editorName, helperFullName, editorFullName, fieldName);
        }

        public static string FormatHtmlHelper_MissingSelectData(string typeName, string expression)
        {
            return string.Format(Resources.HtmlHelper_MissingSelectData, typeName, expression);
        }

        public static string FormatHtmlHelper_WrongSelectDataType(string expression, string? typeName, string expectedTypeName)
        {
            return string.Format(Resources.HtmlHelper_WrongSelectDataType, expression, typeName, expectedTypeName);
        }

        public static string FormatValueProviderResult_NoConverterExists(Type valueType, Type destinationType)
        {
            return string.Format(Resources.ValueProviderResult_NoConverterExists, valueType, destinationType);
        }

        public static string FormatTagHelpers_NoProvidedMetadata(string tagName, string forAttributeName, string providerName, string forName)
        {
            return string.Format(Resources.TagHelpers_NoProvidedMetadata, tagName, forAttributeName, providerName, forName);
        }
    }
}

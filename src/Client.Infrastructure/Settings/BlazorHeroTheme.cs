using MudBlazor;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Settings
{
    public class BlazorHeroTheme
    {
        private static readonly string[] FontStack = ["Inter", "Roboto", "Helvetica", "Arial", "sans-serif"];

        private static readonly Typography DefaultTypography = new()
        {
            Default = new DefaultTypography
            {
                FontFamily = FontStack,
                FontSize = ".875rem",
                FontWeight = "400",
                LineHeight = "1.5",
                LetterSpacing = ".01em"
            },
            H1 = new H1Typography
            {
                FontFamily = FontStack,
                FontSize = "3.5rem",
                FontWeight = "700",
                LineHeight = "1.2",
                LetterSpacing = "-.02em"
            },
            H2 = new H2Typography
            {
                FontFamily = FontStack,
                FontSize = "2.5rem",
                FontWeight = "700",
                LineHeight = "1.25",
                LetterSpacing = "-.01em"
            },
            H3 = new H3Typography
            {
                FontFamily = FontStack,
                FontSize = "2rem",
                FontWeight = "600",
                LineHeight = "1.3",
                LetterSpacing = "0"
            },
            H4 = new H4Typography
            {
                FontFamily = FontStack,
                FontSize = "1.5rem",
                FontWeight = "600",
                LineHeight = "1.35",
                LetterSpacing = ".005em"
            },
            H5 = new H5Typography
            {
                FontFamily = FontStack,
                FontSize = "1.25rem",
                FontWeight = "600",
                LineHeight = "1.4",
                LetterSpacing = "0"
            },
            H6 = new H6Typography
            {
                FontFamily = FontStack,
                FontSize = "1.125rem",
                FontWeight = "600",
                LineHeight = "1.5",
                LetterSpacing = ".005em"
            },
            Button = new ButtonTypography
            {
                FontFamily = FontStack,
                FontSize = ".875rem",
                FontWeight = "600",
                LineHeight = "1.75",
                LetterSpacing = ".02em",
                TextTransform = "none"
            },
            Body1 = new Body1Typography
            {
                FontFamily = FontStack,
                FontSize = "1rem",
                FontWeight = "400",
                LineHeight = "1.6",
                LetterSpacing = ".01em"
            },
            Body2 = new Body2Typography
            {
                FontFamily = FontStack,
                FontSize = ".875rem",
                FontWeight = "400",
                LineHeight = "1.5",
                LetterSpacing = ".01em"
            },
            Caption = new CaptionTypography
            {
                FontFamily = FontStack,
                FontSize = ".75rem",
                FontWeight = "400",
                LineHeight = "1.5",
                LetterSpacing = ".02em"
            },
            Subtitle1 = new Subtitle1Typography
            {
                FontFamily = FontStack,
                FontSize = "1rem",
                FontWeight = "500",
                LineHeight = "1.5",
                LetterSpacing = ".01em"
            },
            Subtitle2 = new Subtitle2Typography
            {
                FontFamily = FontStack,
                FontSize = ".875rem",
                FontWeight = "500",
                LineHeight = "1.5",
                LetterSpacing = ".01em"
            }
        };

        private static readonly LayoutProperties DefaultLayoutProperties = new()
        {
            DefaultBorderRadius = "8px"
        };

        public static readonly MudTheme DefaultTheme = new()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#1565C0",
                PrimaryDarken = "#0D47A1",
                PrimaryLighten = "#42A5F5",
                Secondary = "#00897B",
                Tertiary = "#7E57C2",
                Info = "#0288D1",
                Success = "#2E7D32",
                Warning = "#F57C00",
                Error = "#C62828",
                AppbarBackground = "#FFFFFF",
                AppbarText = "#1C1C1E",
                Background = "#F5F7FA",
                Surface = "#FFFFFF",
                DrawerBackground = "#FFFFFF",
                DrawerText = "#424242",
                DrawerIcon = "#616161",
                TextPrimary = "#1C1C1E",
                TextSecondary = "#5F6368",
                ActionDefault = "#757575",
                Divider = "#E0E0E0",
                DividerLight = "#F0F0F0",
                LinesDefault = "#E0E0E0"
            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties
        };

        public static readonly MudTheme DarkTheme = new()
        {
            PaletteDark = new PaletteDark
            {
                Primary = "#42A5F5",
                PrimaryDarken = "#1E88E5",
                PrimaryLighten = "#90CAF9",
                Secondary = "#4DB6AC",
                Tertiary = "#B39DDB",
                Info = "#29B6F6",
                Success = "#66BB6A",
                Warning = "#FFA726",
                Error = "#EF5350",
                Black = "#1A1A2E",
                Background = "#121218",
                Surface = "#1E1E2D",
                AppbarBackground = "#1E1E2D",
                AppbarText = "rgba(255,255,255, 0.85)",
                DrawerBackground = "#16161F",
                DrawerText = "rgba(255,255,255, 0.65)",
                DrawerIcon = "rgba(255,255,255, 0.50)",
                TextPrimary = "rgba(255,255,255, 0.87)",
                TextSecondary = "rgba(255,255,255, 0.55)",
                ActionDefault = "rgba(255,255,255, 0.60)",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.08)",
                Divider = "rgba(255,255,255, 0.10)",
                DividerLight = "rgba(255,255,255, 0.05)",
                LinesDefault = "rgba(255,255,255, 0.10)"
            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties
        };
    }
}
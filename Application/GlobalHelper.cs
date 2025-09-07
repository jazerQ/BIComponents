namespace Application;

public static class GlobalHelper
{
    public static int OnlyDigits(string word) => int.Parse(string.Join("", word.ToCharArray().Where(char.IsDigit))); 
}
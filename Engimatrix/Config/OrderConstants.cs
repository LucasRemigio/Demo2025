using System.Collections.Generic;
using Engimatrix.ModelObjs;

namespace engimatrix.Config;

public static class OrderConstants
{
    public enum TokenType
    {
        EmailToken,
        OrderToken
    }

    public static string GetColumnName(TokenType tokenColumn)
    {
        return tokenColumn switch
        {
            TokenType.EmailToken => "email_token",
            TokenType.OrderToken => "token",
            _ => throw new ArgumentOutOfRangeException(nameof(tokenColumn), tokenColumn, null)
        };
    }

}

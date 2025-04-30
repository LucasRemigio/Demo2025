// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
namespace engimatrix.Models;

public interface IRatingTypeStrategy
{
    List<RatingTypeItem> Execute(string executerUser);
}

public class ClientRatingTypes : IRatingTypeStrategy
{
    public List<RatingTypeItem> Execute(string executerUser)
    {
        return RatingTypeModel.GetClientRatingTypes(executerUser);
    }
}

public class OrderRatingTypes : IRatingTypeStrategy
{
    public List<RatingTypeItem> Execute(string executerUser)
    {
        return RatingTypeModel.GetOrderRatingTypes(executerUser);
    }
}


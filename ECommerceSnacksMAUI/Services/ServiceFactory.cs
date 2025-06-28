namespace ECommerceSnacksMAUI.Services
{
    public static class ServiceFactory
    {
        public static FavoritesService CreateFavoritesService()
        {
            return new FavoritesService();
        }
    }
}

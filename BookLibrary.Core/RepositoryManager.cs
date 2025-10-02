namespace BookLibrary.Core
{
    public static class RepositoryManager
    {
        private static BookRepository _repository;

        public static BookRepository GetRepository()
        {
            if (_repository == null)
            {
                _repository = new BookRepository();
            }
            return _repository;
        }

        public static void ResetRepository()
        {
            _repository = new BookRepository();
        }
    }
}
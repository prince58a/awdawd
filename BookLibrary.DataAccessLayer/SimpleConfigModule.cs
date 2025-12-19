using Ninject.Modules;
using BookLibrary.Core;

namespace BookLibrary.DataAccessLayer
{
    public class SimpleConfigModule : NinjectModule
    {
        public override void Load()
        {

            string dbPath = Path.Combine("C:\\Users\\gosha\\Documents\\GitHub\\awdawd\\BookLibrary.db");
            Bind<BookDbContext>().ToSelf().InSingletonScope().WithConstructorArgument("dbPath", dbPath);

            // Привязка репозитория книг к реализации EntityRepository
            Bind<IRepository<Book>>().To<EntityRepository>().InSingletonScope();

            // Привязка репозитория жанров к реализации EntityGenreRepository
            Bind<IRepository<Genre>>().To<EntityGenreRepository>().InSingletonScope();

            // Привязка бизнес-логики
            Bind<BookLogic>().ToSelf().InSingletonScope();
        }
    }
}
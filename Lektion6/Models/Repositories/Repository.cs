using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lektion6.Models.Repositories.Abstract;
using Lektion6.Models.Entities;
using Lektion6.Models.Entities.Abstract;
using System.Collections;
using Lektion6.Models.Repositories.Helpers;

namespace Lektion6.Models.Repositories
{
    /// <summary>
    /// Klass som hanterar data för applikationen
    /// </summary>
    public class Repository : IRepository
    {
        // privata listor med User- och Post-objekt
        private Dictionary<string,IList> dataSource;

        /// <summary>
        /// Returns the instance of the repository (Makes sure the repository is only created once - Using the Singleton Design Pattern
        /// </summary>
        public static Repository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot) // Thread-Safe Repository since we are in a multithread environment
                    {
                        if (instance == null)
                            instance = new Repository();
                    }
                }

                return instance;
            }
        }
        private static volatile Repository instance;
        public static object syncRoot = new Object();

        /// <summary>
        /// Privat Konstruktor som genererar 36 dummy Users och 100 dummy Posts
        /// </summary>
        private Repository()
        {
            // Här inne genererar vi dummy-data. För denna lab behöver du inte veta hur denna data genereras.
            List<Guid> UserIDs;
            List<User> users = DummyDataGenerators.GenerateUsers(out UserIDs); // För nyckelordet 'out' se kommentar innan definitionen av GenerateUsers
            List<Post> posts = DummyDataGenerators.GeneratePosts(UserIDs);
            List<ForumThread> forumThreads = new List<ForumThread>();
            List<News> news = DummyDataGenerators.GenerateNews(UserIDs);

            dataSource = new Dictionary<string, IList>();
            dataSource.Add(users.GetType().GetGenericArguments()[0].FullName, users);
            dataSource.Add(posts.GetType().GetGenericArguments()[0].FullName, posts);
            dataSource.Add(forumThreads.GetType().GetGenericArguments()[0].FullName, forumThreads);
            dataSource.Add(news.GetType().GetGenericArguments()[0].FullName, news);
        }

        /// <summary>
        /// Returns a list of the specified Type
        /// </summary>
        /// <typeparam name="T">Valid types: User, Post, News or ForumThread</typeparam>
        /// <returns></returns>
        public List<T> All<T>() where T : IEntity
        {
            if (!dataSource.ContainsKey(typeof(T).FullName))
                throw new Exception("Unsupported Type!");

            return (List<T>)dataSource[typeof(T).FullName];
        }

        /// <summary>
        /// Returns an entity from the datasource with the specified ID or null if the ID doesn't exist
        /// 
        /// Usage: User myUser = myRepository.Get<User>(id);
        /// </summary>
        /// <typeparam name="T">Valid types: User, Post, News or ForumThread</typeparam>
        /// <param name="id">ID of the Entity</param>
        /// <returns></returns>
        public T Get<T>(Guid id) where T : IEntity
        {
            return All<T>().Where(t => t.ID == id).FirstOrDefault();
        }

        public void Delete<T>(T entity) where T : IEntity
        {
            if (!dataSource.ContainsKey(typeof(T).FullName))
                throw new Exception("Unsupported Type!");

            dataSource[typeof(T).FullName].Remove(entity);
        }

        public void Save<T>(T entity) where T : IEntity
        {
            List<T> list = All<T>();

            if (list.Contains(entity))
                list[list.IndexOf(entity)] = entity;
            else
                list.Add(entity);

            dataSource[typeof(T).FullName] = list;
        }

        public List<News> GetLatestNews(int take)
        {
            return All<News>().OrderByDescending(n => n.CreateDate).Take(take).ToList();
        }

        /*
        /// <summary>
        /// Returnerar en lista på alla User-objekt i repositoriet
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            return users;
        }

        /// <summary>
        /// Lägger till en User i repositoriet
        /// </summary>
        /// <param name="newUser">Det User-objekt som skall läggas till</param>
        public void AddUser(User newUser)
        {
            if (newUser.ID == null)
                throw new Exception("UserID is not allowed to be null!");
            if (string.IsNullOrEmpty(newUser.UserName))
                throw new Exception("UserName is not allowed to be empty!");
            if (users.Any(u => u.ID == newUser.ID))
                throw new Exception("A User with that UserID already Exists!");
            users.Add(newUser);
        }

        /// <summary>
        /// Tar bort en befintlig User från Repositoriet
        /// </summary>
        /// <param name="userID">UserID för User-objektet som skall tas bort</param>
        public void RemoveUser(Guid userID)
        {
            if (!users.Any(u => u.ID == userID))
                throw new Exception("User with this UserID does not exist!");
            User userToBeRemoved = users.Where(u => u.ID == userID).FirstOrDefault();
            users.Remove(userToBeRemoved);
        }

        /// <summary>
        /// Returnerar en lista över alla Post-objekt i Repositoriet
        /// </summary>
        /// <returns></returns>
        public List<Post> GetPosts()
        {
            return posts;
        }

        /// <summary>
        /// Läger till ett Post-objekt i repositoriet
        /// </summary>
        /// <param name="newPost">Det Post-objekt som skall läggas till</param>
        public void AddPost(Post newPost)
        {
            if (newPost.ID == null)
                throw new Exception("PostID is not allowed to be null!");
            if (newPost.CreatedByID == null)
                throw new Exception("CreatedByID is not allowed to be null!");
            if (!users.Any(u => u.ID == newPost.CreatedByID))
                throw new Exception("CreatedByID does not match any user!");
            if (string.IsNullOrEmpty(newPost.Body))
                throw new Exception("Body is not allowed to be empty!");
            if (newPost.CreateDate == null || newPost.CreateDate == new DateTime())
                throw new Exception("CreateDate must be set");
            if (posts.Any(p => p.ID == newPost.ID))
                throw new Exception("A Post with that PostID already Exists!");
            posts.Add(newPost);
        }

        /// <summary>
        /// Tar bort ett Post-objekt från Repositoriet
        /// </summary>
        /// <param name="postID">PostID för det Post-objekt som skall tas bort</param>
        public void RemovePost(Guid postID)
        {
            if (!posts.Any(p => p.ID == postID))
                throw new Exception("Post with this PostID does not exist!");
            Post postToBeRemoved = posts.Where(p => p.ID == postID).FirstOrDefault();
            posts.Remove(postToBeRemoved);
        }
         * */
    }
}

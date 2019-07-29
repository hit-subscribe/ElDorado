using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Trello
{
    public interface ITrelloSynchronizer<T> where T : class, IHaveIdentity, new()
    {
        void Initialize(string filePath);
        void Initialize(CredentialStore credentialStore);
        void CreateCardForEntity(T entity);
        void UpdateCardForEntity(T entity);
        void DeleteCard(string trellId);
        bool DoesCardExistWithTitle(string title);
    }
}

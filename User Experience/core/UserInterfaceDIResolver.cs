using Resolver = VContainer.IObjectResolver;

namespace ATG.UI
{
    public sealed class UserInterfaceDIResolver
    {
        private readonly Resolver _resolver;

        public UserInterfaceDIResolver(Resolver resolver)
        {
            _resolver = resolver;
        }

        public bool TryResolve<T>(out T solution)
        {
            object resolved;

            bool isResolve = _resolver.TryResolve(typeof(T), out resolved);
            
            if(isResolve == true)
            {
                solution = (T)resolved;
                return true;
            }

            solution = default;
            return false;
        }
    }
}
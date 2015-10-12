namespace SmartExpense.UI.Helpers
{
    using Microsoft.Practices.Unity;

    public static class UnityContainerHelper
    {
        public static void RegisterContainerControlled<TInterface, TInstance>(this UnityContainer container)
            where TInstance : TInterface
        {
            container.RegisterType<TInterface, TInstance>(new ContainerControlledLifetimeManager());
        }

        public static void RegisterContainerControlled<TInstance>(this UnityContainer container)
        {
            container.RegisterType<TInstance>(new ContainerControlledLifetimeManager());
        }

        public static void RegisterPerResolve<TInterface, TInstance>(this UnityContainer container)
            where TInstance : TInterface
        {
            container.RegisterType<TInterface, TInstance>(new PerResolveLifetimeManager());
        }

        public static void RegisterPerResolve<TInstance>(this UnityContainer container)
        {
            container.RegisterType<TInstance>(new PerResolveLifetimeManager());
        }
    }
}

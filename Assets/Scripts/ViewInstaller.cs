using Radial;
using UnityEngine;
using Zenject;

public class ViewInstaller : MonoInstaller
{
    [SerializeField] private RadialMenu radialMenu;

    public override void InstallBindings()
    {
        Container.Bind<RadialMenu>().FromInstance(radialMenu).AsSingle();
        //Container.InstantiatePrefabForComponent<RadialMenu>();
    }
}


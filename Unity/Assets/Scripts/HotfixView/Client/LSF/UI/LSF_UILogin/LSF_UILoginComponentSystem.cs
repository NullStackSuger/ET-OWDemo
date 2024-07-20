using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSF_UILoginComponent))]
    [FriendOf(typeof(LSF_UILoginComponent))]
    public static partial class LSF_UILoginComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSF_UILoginComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.loginBut = rc.Get<GameObject>("LoginBut");

            self.loginBut.GetComponent<Button>().onClick.AddListener(self.OnLogin);
            self.email = rc.Get<GameObject>("Email");
            self.password = rc.Get<GameObject>("PassWord");
        }


        private static void OnLogin(this LSF_UILoginComponent self)
        {
            LoginHelper.Login(self.Root(),
                self.email.GetComponent<InputField>().text,
                self.password.GetComponent<InputField>().text).Coroutine();
        }
    }
}
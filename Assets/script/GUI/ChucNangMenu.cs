using UnityEngine;
using UnityEngine.SceneManagement;
public class ChucNangMenu : MonoBehaviour
{
    public void ChoiMoi()
    {
        SceneManager.LoadScene(4);
    }
    public void TaoTK() 
    {
        SceneManager.LoadScene(1);
    }
    public void DangNhap()
    {
        SceneManager.LoadScene(2);
    }
    public void Thoat()
    { 
    Application.Quit();
    }
}

//������Ʈ�� ������ ������ ���� �̿���
public interface IOnOff
{
    public bool Active { get; set; }
    public bool On();

    public bool Off();
}

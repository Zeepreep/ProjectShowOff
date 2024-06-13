using UnityEngine;

public class StringScript : MonoBehaviour
{
    public PhotoSpot end1;
    public PhotoSpot end2;

    public void SetEnds(PhotoSpot end1, PhotoSpot end2)
    {
        this.end1 = end1;
        this.end2 = end2;
    }

    public void UpdateString()
    {
        if (end1 != null && end2 != null)
        {
            Vector3 position = (end1.transform.position + end2.transform.position) / 2;
            Vector3 direction = end1.transform.position - end2.transform.position;
            Quaternion rotationOffset = Quaternion.Euler(-90, 0, 0);
            Quaternion rotation = Quaternion.LookRotation(direction) * rotationOffset;
            float distance = Vector3.Distance(end1.transform.position, end2.transform.position);

            Vector3 scale = new Vector3(1f, (distance / transform.parent.lossyScale.y) / 2, 1f);

            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

public struct PathFindingRect
{
    public Rect rect;
    public List<Rect> obstainsRect;
}

public class MovementManager : Manager<MovementManager>
{
    [SerializeField]
    private BoxCollider2D map = null;
    [SerializeField]
    private BoxCollider2D player = null;
    [SerializeField]
    private BoxCollider2D[] objects = null; // 이 안에 들어오는 모든 콜리더의 offset은 (0, 0)이어야 한다.
    [SerializeField]
    private float speed = 0.1f;

    private List<Rect> rects = new List<Rect>();
    private const int TILE_SIZE = 10;
    private List<List<PathFindingRect>> TileMap;

	// Use this for initialization
	void Start ()
    {
        if (map == null)
            this.enabled = false;

        if (GameManager.IsInited)
            player = GameManager.Instance.Player.transform.GetChild(0).GetComponent<BoxCollider2D>();
        else
            this.enabled = false;

        for(int i = 0; i < objects.Length; i += 1)
        {
            Rect rect = new Rect((Vector2)objects[i].transform.position, objects[i].size);

            rects.Add(rect);
        }
	}

    bool IntersectRect(Rect rect1, Rect rect2)
    {
        if (rect1.xMin < rect2.xMax && rect1.xMax > rect2.xMin &&
            rect1.yMin < rect2.yMax && rect1.yMax > rect2.yMin)
            return true;
        else
            return false;
    }

    void FindPath(Vector2 pos)
    {

    }
}

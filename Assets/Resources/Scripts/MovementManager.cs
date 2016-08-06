using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Wintellect.PowerCollections;
using System;

public class PathInfo : IComparable<PathInfo>
{
    public Vector2 pos;
    public PathInfo befPath;

    public PathInfo()
    {
        pos = Vector3.zero;
        befPath = null;
    }

    public PathInfo(Vector2 pos, PathInfo befPath)
    {
        this.pos = pos;
        this.befPath = befPath;
    }

    int IComparable<PathInfo>.CompareTo(PathInfo other)
    {
        return 0;
    }
}

public class MovementManager : Manager<MovementManager>
{
    private BoxCollider2D map = null;
    private BoxCollider2D player = null;
    private Animator playerAnimation = null;

    private float speed = 500f;

    private float movementDistance = 10f;

    private List<Rect> rects = new List<Rect>();
    private const int TILE_SIZE = 10;
    private Rect playerRect;
    private Rect mapRect;

    private float sin45;
    private float cos45;
    private float sin135;
    private float cos135;
    private float sinM45;
    private float cosM45;
    private float sinM135;
    private float cosM135;

    private List<Vector2> path;
    private bool findingPath = false;

    private Vector2 oriStartPos;
    private bool pathMoving = false;
    private float lerpRate = 0f;
    private float oriDis = 0f;
    private bool gameScene = false;

    [SerializeField]
    private GameObject DebuggingObj = null;

    // Use this for initialization
    void Start()
    {
        Instance.Init();

        sin45 = Mathf.Sin(45);
        cos45 = Mathf.Cos(45);
        sin135 = sin45;
        cos135 = -cos45;
        sinM45 = -sin45;
        cosM45 = cos45;
        sinM135 = -sin45;
        sinM135 = -cos45;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            == SceneManager.Instance.GetLevel("GameScene"))
        {
            InitGameScene();
        }
    }

    public void update()
    {
        if (gameScene)
        {
            CheckInput();

            if (path != null)
                GoByPath();
        }

    }

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);

        if (level == SceneManager.Instance.GetLevel("GameScene"))
        {
            InitGameScene();
        }
        else
        {
            gameScene = false;
            path = null;
        }
    }

    public void InitGameScene()
    {
        gameScene = true;
        player = GameManager.Instance.Player.GetComponent<BoxCollider2D>();
        playerAnimation = player.GetComponent<Animator>();
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<BoxCollider2D>();

        var objs = GameObject.FindGameObjectsWithTag("Furnitures");
        for (int i = 0; i < objs.Length; i += 1)
        {
            BoxCollider2D collider = objs[i].GetComponent<BoxCollider2D>();

            Rect rect = new Rect();
            rect.position = (Vector2)collider.transform.localPosition + collider.offset;
            rect.size = collider.size;
            rects.Add(rect);
        }

        playerRect = new Rect();
        playerRect.position = (Vector2)player.transform.localPosition + player.offset;
        playerRect.size = player.size;
        mapRect = new Rect();
        mapRect.position = (Vector2)map.transform.localPosition + map.offset;
        mapRect.size = map.size;
    }

    private void GoByPath()
    {
        if (!pathMoving)
        {
            pathMoving = true;

            oriStartPos = player.transform.localPosition;
            oriDis = Vector2.Distance(oriStartPos, path[0]);
            lerpRate = 0f;
        }

        lerpRate += (speed * Time.smoothDeltaTime) / oriDis;

        float oriX = player.transform.localPosition.x;
        player.transform.localPosition = Vector2.Lerp(oriStartPos, path[0], lerpRate);
        float x = player.transform.localPosition.x;

        playerAnimation.SetFloat("xSpeed", x - oriX);

        if (lerpRate >= 1f)
        {
            pathMoving = false;

            if (path.Count > 1)
                path.RemoveAt(0);
            else
            {
                path = null;
                playerAnimation.SetFloat("xSpeed", 0);
            }
        }
    }

    private bool IntersectRect(Rect rect1, Rect rect2)
    {
        float xMin1 = rect1.position.x - rect1.size.x / 2;
        float xMax1 = rect1.position.x + rect1.size.x / 2;
        float yMin1 = rect1.position.y - rect1.size.y / 2;
        float yMax1 = rect1.position.y + rect1.size.y / 2;

        float xMin2 = rect2.position.x - rect2.size.x / 2;
        float xMax2 = rect2.position.x + rect2.size.x / 2;
        float yMin2 = rect2.position.y - rect2.size.y / 2;
        float yMax2 = rect2.position.y + rect2.size.y / 2;

        if (xMin1 < xMax2 && xMax1 > xMin2 &&
            yMin1 < yMax2 && yMax1 > yMin2)
            return true;
        else
            return false;
    }

    private Vector2[] GetPossibleDes(Vector2 pos)
    {
        Vector2[] paths = new Vector2[8];

        paths[0] = new Vector2(pos.x, pos.y + movementDistance);
        paths[1] = new Vector2(pos.x, pos.y - movementDistance);
        paths[2] = new Vector2(pos.x + movementDistance, pos.y);
        paths[3] = new Vector2(pos.x - movementDistance, pos.y);
        paths[4] = new Vector2(pos.x + movementDistance * cos45, pos.y + movementDistance * sin45);
        paths[5] = new Vector2(pos.x + movementDistance * cos135, pos.y + movementDistance * sin135);
        paths[6] = new Vector2(pos.x + movementDistance * cosM45, pos.y + movementDistance * sinM45);
        paths[7] = new Vector2(pos.x + movementDistance * cosM135, pos.y + movementDistance * sinM135);

        return paths;
    }

    private bool CollisionCheck()
    {
        foreach (var iter in rects)
        {
            if (IntersectRect(iter, playerRect))
                return true;
        }

        return false;
    }

    private void MakePath(PathInfo desPath)
    {
        path = new List<Vector2>();
        PathInfo curPath = desPath;

        while (curPath.befPath != null)
        {
            path.Add(curPath.pos);

            curPath = curPath.befPath;
        }

        path.Reverse();
    }

    IEnumerator FindPath(Vector2 startPos, Vector2 desPos)
    {
        playerRect.position = desPos + player.offset;
        bool arrive = false;

        if (CollisionCheck())
        {
            Debug.LogWarning("Player Can Not Go There");
            findingPath = false;
            StopAllCoroutines();
            PathFindingFailed();
            arrive = true;
        }

        float time = 0f;
        findingPath = true;
        startPos.x = Mathf.Round(startPos.x);
        startPos.y = Mathf.Round(startPos.y);

        OrderedMultiDictionary<float, PathInfo> openPaths = new OrderedMultiDictionary<float, PathInfo>(true);
        List<PathInfo> closedPath = new List<PathInfo>();

        Dictionary<Vector2, PathInfo> S = new Dictionary<Vector2, PathInfo>();

        openPaths.Add(Mathf.Round(Vector2.Distance(startPos, desPos)), new PathInfo(startPos, null));
        S.Add(startPos, new PathInfo(startPos, null));

        while (!arrive && openPaths.Count > 0)
        {
            time += Time.smoothDeltaTime;

            if(time >= 1.0f)
            {
                StopAllCoroutines();
                PathFindingFailed();
                break;
            }

            for (int i = 0; i < 50; i += 1)
            {
                KeyValuePair<float, PathInfo> curPath = new KeyValuePair<float, PathInfo>();
                foreach (var iter in openPaths)
                {
                    float key = iter.Key;
                    PathInfo info = null;
                    foreach (var valIter in iter.Value)
                    {
                        info = valIter;
                        break;
                    }
                    curPath = new KeyValuePair<float, PathInfo>(key, info);

                    break;
                }

                openPaths.Remove(curPath.Key);


                if (DebuggingObj != null)
                    DebuggingObj.transform.localPosition = curPath.Value.pos;

                if (Vector2.Distance(curPath.Value.pos, desPos) <= movementDistance)
                {
                    desPos.x = Mathf.Round(desPos.x);
                    desPos.y = Mathf.Round(desPos.y);

                    findingPath = false;
                    MakePath(new PathInfo(desPos, curPath.Value));
                    arrive = true;
                    StopAllCoroutines();
                    break;
                }

                var paths = GetPossibleDes(curPath.Value.pos);

                for (int j = 0; j < 8; j += 1)
                {
                    paths[j].x = Mathf.Round(paths[j].x);
                    paths[j].y = Mathf.Round(paths[j].y);


                    playerRect.position = paths[j] + player.offset;
                    if (CollisionCheck())
                        continue;

                    float distance = Vector2.Distance(paths[j], desPos);


                    if (S.ContainsKey(paths[j]))
                    {
                        PathInfo info;
                        S.TryGetValue(paths[j], out info);

                        if (info.befPath != null)
                        {
                            float otherDis = Vector2.Distance(info.pos, desPos);
                            float thisDis = Vector2.Distance(paths[j], desPos);

                            if (thisDis < otherDis)
                            {
                                S.Remove(paths[j]);
                                S.Add(paths[j], new PathInfo(paths[j], curPath.Value));

                                

                                openPaths.Remove(Mathf.Round(otherDis));
                                openPaths.Add(Mathf.Round(thisDis), new PathInfo(paths[j], curPath.Value));
                            }
                        }
                    }
                    else
                    {
                        if (DebuggingObj != null)
                            DebuggingObj.transform.localPosition = paths[j];

                        openPaths.Add(Mathf.Round(Vector2.Distance(paths[j], desPos)), new PathInfo(paths[j], curPath.Value));
                    }
                }

            }

            yield return null;
        }
    }

    private bool ContainsPoint(Rect rect, Vector2 point)
    {
        float xMin = rect.position.x - rect.size.x / 2;
        float xMax = rect.position.x + rect.size.x / 2;
        float yMin = rect.position.y - rect.size.y / 2;
        float yMax = rect.position.y + rect.size.y / 2;

        if (point.x >= xMin && point.x <= xMax &&
            point.y >= yMin && point.y <= yMax)
            return true;
        else
            return false;
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (ContainsPoint(mapRect, worldPos))
            {

                if (findingPath)
                    StopAllCoroutines();

                StartCoroutine(FindPath(player.transform.localPosition, worldPos));
            }

        }
    }

    public void PathFindingFailed()
    {
        playerAnimation.SetBool("Embrassing", true);
        enabled = false;
    }
}

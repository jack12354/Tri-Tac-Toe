using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    private static readonly int Width = 3;
    private static readonly int Height = 3;
    private static readonly int Depth = 3;
    private static readonly float WidthSize = 2.0f;
    private static readonly float HeightSize = 2.0f;
    private static readonly float DepthSize = 2.0f;

    Peice[,,] mPeices = new Peice[Width, Height, Depth];
    private int mCurrentPlayer = 0;
    [Range(1, 8)]
    public int Players = 2;

    private string inputString = "";
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Height >= 1 && Input.GetKeyDown(KeyCode.Alpha1)) { inputString += 1.ToString(); }
        if (Height >= 2 && Input.GetKeyDown(KeyCode.Alpha2)) { inputString += 2.ToString(); }
        if (Height >= 3 && Input.GetKeyDown(KeyCode.Alpha3)) { inputString += 3.ToString(); }
        if (Height >= 4 && Input.GetKeyDown(KeyCode.Alpha4)) { inputString += 4.ToString(); }
        if (Height >= 5 && Input.GetKeyDown(KeyCode.Alpha5)) { inputString += 5.ToString(); }
        if (Height >= 6 && Input.GetKeyDown(KeyCode.Alpha6)) { inputString += 6.ToString(); }
        if (Height >= 7 && Input.GetKeyDown(KeyCode.Alpha7)) { inputString += 7.ToString(); }
        if (Height >= 8 && Input.GetKeyDown(KeyCode.Alpha8)) { inputString += 8.ToString(); }
        if (Height >= 9 && Input.GetKeyDown(KeyCode.Alpha9)) { inputString += 9.ToString(); }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DestroyPeice();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlaceAll();
        }

        if (inputString.Length == 3)
        {
            int x = int.Parse(inputString[0].ToString()) - 1;
            int y = int.Parse(inputString[1].ToString()) - 1;
            int z = int.Parse(inputString[2].ToString()) - 1;
            if (!IsOccupied(x, y, z))
            {
                CreatePeice(x, y, z);
                mCurrentPlayer++;
                mCurrentPlayer %= Players;
                switch (CheckWins())
                {
                    case -1: Debug.Log("Nobody Wins"); break;
                    case 0: Debug.Log("Red Wins"); break;
                    case 1: Debug.Log("Blue Wins"); break;
                    case 2: Debug.Log("Green Wins"); break;
                    case 3: Debug.Log("Yellow Wins"); break;
                }
            }
            inputString = "";
        }

        ///transform.Rotate(Vector3.up, Mathf.PI / 6.0f);
    }

    private void PlaceAll()
    {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                for (int z = 0; z < Depth; z++)
                    if (!IsOccupied(x, y, z) && Random.value < 0.3f)
                    {
                        CreatePeice(x, y, z);
                        mCurrentPlayer++;
                        mCurrentPlayer %= Players;
                    }
    }

    private void DestroyPeice()
    {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                for (int z = 0; z < Depth; z++)
                {
                    if (IsOccupied(x, y, z))
                    {
                        var spawnedRigidbody = mPeices[x, y, z].gameObject.AddComponent<Rigidbody>();
                        float force = 70.0f;
                        spawnedRigidbody.AddForce(new Vector3(Random.Range(-force, force), Random.Range(-force, force), Random.Range(-force, force)));
                        //spawnedRigidbody.AddExplosionForce(force, GetPeicePosition(new Position(Width/2, Height/2, Depth/2)), 100, 0);
                        //Destroy(mPeices[x, y, z].gameObject.GetComponent<Collider>());
                        Destroy(mPeices[x, y, z].gameObject, Random.Range(4.0f, 5.0f));
                        mPeices[x, y, z] = null;
                    }
                }

    }

    private void CreatePeice(int inX, int inY, int inZ)
    {
        var go = Instantiate(Resources.Load("Node")) as GameObject;
        go.GetComponent<Peice>().Init(this, mCurrentPlayer, inX, inY, inZ);
        mPeices[inX, inY, inZ] = go.GetComponent<Peice>();
    }

    int CheckWins()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                bool unbroken = true;
                int team = GetTeam(x, y, 0);
                for (int z = 1; z < Depth; z++)
                {
                    if (team == -1 || team != GetTeam(x, y, z))
                    {
                        unbroken = false;
                        break;
                    }
                }
                if (unbroken)
                    return team;

            }
        }

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Depth; z++)
            {
                bool unbroken = true;
                int team = GetTeam(x, 0, z);
                for (int y = 1; y < Height; y++)
                {
                    if (team == -1 || team != GetTeam(x, y, z))
                    {
                        unbroken = false;
                        break;
                    }
                }
                if (unbroken)
                    return team;
            }
        }

        for (int y = 0; y < Height; y++)
        {
            for (int z = 0; z < Depth; z++)
            {
                bool unbroken = true;
                int team = GetTeam(0, y, z);
                for (int x = 1; x < Width; x++)
                {
                    if (team == -1 || team != GetTeam(x, y, z))
                    {
                        unbroken = false;
                        break;
                    }
                }
                if (unbroken)
                    return team;
            }
        }

        return -1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                for (int z = 0; z < Depth; z++)
                {
                    if (!IsOccupied(x, y, z))
                        Gizmos.DrawWireSphere(GetPeicePosition(new Position(x, y, z)), 0.5f);
                }

        Gizmos.color = Color.red;
        foreach (var peice in mPeices)
        {
            if (peice != null)
                Gizmos.DrawWireCube(GetPeicePosition(peice.position), Vector3.one);
        }

        Gizmos.color = Color.green;
        int selectionx = -1;
        int selectiony = -1;
        int selectionz = -1;
        if (inputString.Length >= 1) selectionx = int.Parse(inputString[0].ToString()) - 1;
        if (inputString.Length >= 2) selectiony = int.Parse(inputString[1].ToString()) - 1;
        if (inputString.Length >= 3) selectionz = int.Parse(inputString[2].ToString()) - 1;

        Position center = Position.Zero;

        Position size = new Position((int) (Width * WidthSize)/2, (int) (Height * HeightSize)/2, (int) (Depth * DepthSize)/2);
        if (selectionx != -1)
        {
            center.X = selectionx;
            size.X = 1;
        }
        else
        {
            center.X = (Width - 1) / 2;
        }
        if (selectiony != -1)
        {
            center.Y = selectiony;
            size.Y = 1;
        }
        else
        {
            center.Y = (Height - 1) / 2;
        }
        if (selectionz != -1)
        {
            center.Z = selectionz;
            size.Z = 1;
        }
        else
        {
            center.Z = (Depth - 1) / 2;
        }
        Gizmos.DrawWireCube(GetPeicePosition(center), new Vector3(size.X * WidthSize, size.Y * HeightSize, size.Z * DepthSize));
    }

    public Vector3 GetPeicePosition(Position inPosition)
    {
        return transform.position + new Vector3(inPosition.X * WidthSize, inPosition.Y * HeightSize, inPosition.Z * DepthSize);
        // - (new Vector3(Width * WidthSize, Height * HeightSize, Depth * DepthSize) / 2.0f);
    }

    public bool IsOccupied(int inX, int inY, int inZ)
    {
        return mPeices[inX, inY, inZ] != null;
    }

    public int GetTeam(int inX, int inY, int inZ)
    {
        if (mPeices[inX, inY, inZ] == null)
            return -1;
        return mPeices[inX, inY, inZ].team;
    }
}

public struct Position
{
    public static Position Zero = new Position(0, 0, 0);
    public static Position Down = new Position(0, -1, 0);
    public static Position Up = new Position(0, 1, 0);
    public static Position Left = new Position(-1, 0, 0);
    public static Position Right = new Position(1, 0, 0);
    public static Position In = new Position(0, 0, 1);
    public static Position Out = new Position(0, 0, -1);

    public Position(int inX, int inY, int inZ)
    {
        X = inX;
        Y = inY;
        Z = inZ;
    }

    //public void Add(Position inPosition)
    //{
    //    X += inPosition.X;
    //    Y += inPosition.Y;
    //    Z += inPosition.Z;
    //}

    public override string ToString()
    {
        return "{" + X + " " + Y + " " + Z + "}";

        ;
    }

    public int X, Y, Z;
}


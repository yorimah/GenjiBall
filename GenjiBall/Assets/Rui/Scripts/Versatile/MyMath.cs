using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AddMath
{
    public class MyMath : MonoBehaviour
    {
        public static int Sign(float value)
        {
            if (value > 0) return 1;
            if (value < 0) return 0;
            return 0;
        }
        public static int wrap(int value, int low, int high)
        {
            if (high < low) { Debug.LogError("highはlowより大きな数にしてください"); return value; }

            int n = (value - low) % (high - low);
            return (n >= 0) ? (n + low) : (n + high);
        }

        public static float Distance(Vector3 vec1, Vector3 vec2)
        {
            return (vec1 - vec2).magnitude;
        }

        public static Vector3 Direction(Vector3 vec1, Vector3 vec2)
        {
            return (vec1 - vec2).normalized;
        }

        // ベクトルから角度に変換
        public static float ConvtoAngle(Vector2 vec)
        {
            return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
        }

        // 角度からベクトルに変換
        public static Vector3 ConvtoDir(float angle)
        {
            Vector3 result = Vector3.zero;
            result.x = Mathf.Sin(angle * Mathf.Deg2Rad);
            result.z = Mathf.Cos(angle * Mathf.Deg2Rad);
            return result.normalized;
        }
        public static Vector2 Returnone(Vector2 vec, int witch)
        {
            Vector2 dir = Vector2.zero;
            if (witch == 0) dir.x = vec.x;
            else if (witch == 1) dir.y = vec.y;
            return dir;
        }

        // vecに入った値からplaceに入った指定場所を０にして返す
        public static Vector3 ConvtoTwoVec(Vector3 vec, int place)
        {
            if (place < 0 || place > 2)
            {
                Debug.LogError("placeが-1以下または、3以上が指定されています");
                return vec;
            }

            Vector3 result = vec;
            switch (place)
            {
                case 0:
                    result.x = 0;
                    break;
                case 1:
                    result.y = 0;
                    break;
                case 2:
                    result.z = 0;
                    break;
            }
            return result;
        }

        // ある座標targetposからtargetposから見た方向の座標dirposにcorrectvalue分移動した座標を返す
        public static Vector3 CorrectPos(Vector3 targetpos, Vector3 dirpos, float correctvalue)
        {
            targetpos += (Direction(dirpos, targetpos)) * correctvalue;
            return targetpos;
        }

        public static float RandomNum(float[] num)
        {
            return num[Random.Range(0, num.Length)];
        }

        // vector3の型で、それぞれの要素をランダムにして返す
        public static Vector3 RandomVec3(float min, float max)
        {
            Vector3 result;
            result.x = Random.Range(min, max);
            result.y = Random.Range(min, max);
            result.z = Random.Range(min, max);
            return result;
        }

        // vector3の型で、xとｚの要素をランダムにして返す
        public static Vector3 RandomPlaneVec(float min, float max)
        {
            Vector3 result = RandomVec3(min, max);
            result.y = 0;
            return result;
        }

        // vector3の型で、それぞれの要素をランダムにして返す
        public static Vector3 RandomPlaneVec3Value()
        {
            Vector3 result;
            result.x = Random.value * 2 - 1;
            result.y = 0;
            result.z = Random.value * 2 - 1;
            return result;
        }

        public static Vector3 signVector(Vector3 vec)
        {
            Vector3 result;
            result.x = Mathf.Sign(vec.x);
            result.y = Mathf.Sign(vec.y);
            result.z = Mathf.Sign(vec.z);

            return result;
        }

        // numが123.459でdecが2だったら123.46となる
        public static float Floor(float num, int dec)
        {
            int n = (int)(num * Mathf.Pow(10, dec));
            return n / Mathf.Pow(10, dec);
        }

        public static Vector3 vectorFloor(Vector3 vec, int dec)
        {
            Vector3 result;
            result.x = Floor(vec.x, dec);
            result.y = Floor(vec.y, dec);
            result.z = Floor(vec.z, dec);

            return result;
        }

        public static float Round(float num, int dec)
        {
            float n = Mathf.Pow(10, dec);
            return Mathf.Round(num * n) / n;
        }

        public static Vector3 Round(Vector3 vec, int dec)
        {
            Vector3 result;
            result.x = Round(vec.x, dec);
            result.y = Round(vec.y, dec);
            result.z = Round(vec.z, dec);

            return result;
        }

        // ベクトルからsinの値を取得する
        public static float getSin(Vector2 vector)
        {
            float value = vector.y / vector.magnitude;
            return value;
        }

        // ベクトルtargetDirのdownDIr方向の要素が０になった時のベクトルを返す
        public static Vector3 getHorizontal(Vector3 targetDir, Vector3 downDir)
        {
            targetDir = targetDir.normalized;
            downDir = downDir.normalized;

            Vector3 right = (Vector3.Cross(-targetDir, downDir).normalized);

            // ２つのベクトルの間の角度を求め、その角度と９０度と、内角が１８０度というのを使ってもう１つの角度を求める
            float thetaAngle = 90 - Vector3.SignedAngle(-targetDir, downDir, right);

            // 求めた角度の時にtargetDIr高さが０になるにはどのくらい下げる(高さ)のかを求める
            float dis_B = Mathf.Sin(Mathf.Deg2Rad * thetaAngle);

            // 基準となるベクトルに下向き方向を足して高さをなくしたベクトルを求める
            Vector3 result = targetDir + (downDir * dis_B);

            return result.normalized;
        }

        // 引数を使って特定の角度の水平ベクトルを求め、targetDirと角度を比べて返す
        public static float getAngle(Vector3 targetDir, Vector3 downDir)
        {
            Vector3 horizontal = getHorizontal(targetDir, downDir);
            Vector3 dir = (Vector3.Cross(-targetDir, downDir).normalized);

            return Vector3.SignedAngle(horizontal, targetDir, dir);
        }

        public static bool checkHitBox(Vector3 pos1, Vector3 size1, Vector3 pos2, Vector3 size2)
        {
            Vector2 p1 = getVertexPoint(pos1, size1, false, false); // 左上
            Vector2 p2 = getVertexPoint(pos1, size1, true, true); // 右下
            Vector2 q1 = getVertexPoint(pos2, size2, false, false); // 左上 
            Vector2 q2 = getVertexPoint(pos2, size2, true, true); // 右下 

            return checkHitObject(p1.x, p2.x, q1.x, q2.x) && checkHitObject(p1.y, p2.y, q1.y, q2.y);
        }

        // 当たり判定の頂点の座標を得るための関数　座標、サイズ、ｘはtrueで右、falseで左　ｙはtrueで上、falseで下
        public static Vector2 getVertexPoint(Vector2 pos, Vector2 size, bool x, bool y)
        {
            Vector2 result = pos;
            if (x == true) { result.x += size.x * 0.5f; }
            else { result.x -= size.x * 0.5f; }
            if (y == true) { result.y += size.y * 0.5f; }
            else { result.y -= size.y * 0.5f; }

            return result;
        }
        // 当たり判定の頂点の座標を得るための関数　座標、サイズ、ｘはtrueで右、falseで左　ｙはtrueで上、falseで下　ｚはtrueで奥、falseで手前
        public static Vector3 getVertexPoint(Vector3 pos, Vector3 size, bool x, bool y, bool z)
        {
            Vector3 result = getVertexPoint(pos, size, x, y);
            
            if (z == true) { result.z += size.z * 0.5f; }
            else { result.z -= size.z * 0.5f; }

            return result;
        }

        public static bool checkHitObject(float p1, float p2, float q1, float q2)
        {
            bool result = (p2 >= q1 && q2 >= p1);
            return (p2 >= q1 && q2 >= p1);
        }

        public static bool circleRectangleCollision(Vector3 CirclePosition, float range, Vector3 rectanglePosition, Vector3 size)
        {
            Vector3 position = Vector3.zero; // 円と四角形の辺上の座標との最短座標
            size *= 0.5f;

            position.x = Mathf.Clamp(CirclePosition.x, rectanglePosition.x - size.x, rectanglePosition.x + size.x);
            position.y = CirclePosition.y;
            position.z = Mathf.Clamp(CirclePosition.z, rectanglePosition.z - size.z, rectanglePosition.z + size.z);
            Vector3 toshortestCoordinatesVector = position - CirclePosition;
            return (toshortestCoordinatesVector.sqrMagnitude < range * range);
        }

        public static bool checkArray(bool[] arrays, bool judge)
        {
            foreach (bool array in arrays)
            {
                if (array != judge) return false;
            }
            return true;
        }
    }
}
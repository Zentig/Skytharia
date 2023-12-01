using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Unity.VisualScripting; // Why the hell is commaseperatedstring a VisualScripting func?
#endif

public static class SpellSystemUtlities 
{
    public static Vector2[] ScalePointGrouping(Vector2[] points, float horizontalLen, float verticalLen)
    {
        (Vector2 leftMost, Vector2 rightMost, Vector2 upMost, Vector2 downMost) GetBoundPoints(Vector2[] collection)
        {
           Vector2 leftMost = new(Mathf.Infinity, 0f);
           Vector2 rightMost = new(-Mathf.Infinity, 0f);
           Vector2 upMost = new(0f, -Mathf.Infinity);
           Vector2 downMost = new(0f, Mathf.Infinity);
           // ok this is pretty bad but idrc because it works, can optimise later its O(n) anyway im pretty sure
           foreach (var vector in collection)
           {
                if (vector.x < leftMost.x)
                {
                    leftMost = vector;
                }
                if (vector.x > rightMost.x)
                {
                    rightMost = vector;
                }
                if (vector.y > upMost.y)
                {
                    upMost = vector;
                }
                if (vector.y < downMost.y)
                {
                    downMost = vector;
                }
            }
            return (leftMost, rightMost, upMost, downMost);
        }
        Vector2[] output = new Vector2[points.Length];
        var bounds = GetBoundPoints(points);
        float horizontalScaleFactor = horizontalLen / (bounds.rightMost.x - bounds.leftMost.x);
        float verticalScaleFactor = verticalLen / (bounds.upMost.y - bounds.downMost.y);
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 toScale = points[i];
            Vector2 scaled = new(toScale.x * horizontalScaleFactor, toScale.y * verticalScaleFactor);
            output[i] = scaled;
        }
        return output;
    }
    #if UNITY_EDITOR
    [MenuItem("Debug/Spell System/TestPointScaler")]
    private static void Debug_TestPointScaler()
    {
        int pointsToGenerate = 5;
        float desiredHorizontalLen = 2f;
        float desiredVerticalLen = 2f;
        Vector2[] pointsToScale = new Vector2[pointsToGenerate];
        for (int i = 0; i < pointsToScale.Length; i++)
        {
            float x = Random.Range(-20f, 20f);
            float y = Random.Range(-20f, 20f);
            pointsToScale[i] = new(x, y);
        }
        Vector2[] output = ScalePointGrouping(pointsToScale, desiredHorizontalLen, desiredVerticalLen);
        Debug.Log($"Test Input: Generate {pointsToGenerate} points, Scale to {desiredHorizontalLen} by {desiredVerticalLen}. Points: {pointsToScale.ToCommaSeparatedString()} \n Test Output: {output.ToCommaSeparatedString()}");
    }
    #endif
}
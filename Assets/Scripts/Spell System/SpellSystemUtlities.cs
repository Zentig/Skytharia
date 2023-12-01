using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Unity.VisualScripting; // Why the hell is commaseperatedstring a VisualScripting func?
#endif

public static class SpellSystemUtlities 
{
    public static Vector2[] ScalePointGrouping(Vector2[] points, float horizontalLen, float verticalLen)
    {
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
    public static Vector2[] CenterPointGrouping(Vector2[] points, Vector2 centreAround)
    {
        var bounds = GetBoundPoints(points);
        Vector2[] output = new Vector2[points.Length];
        float xCenter = (bounds.rightMost.x + bounds.leftMost.x) / 2;
        float yCenter = (bounds.upMost.y + bounds.downMost.y) / 2; 
        float xRequiredOffset = xCenter - centreAround.x;
        float yRequiredOffset = yCenter - centreAround.y;
        Vector2 offset = new(xRequiredOffset, yRequiredOffset);
        for (int i = 0; i < points.Length; i++)
        {
            float newX = points[i].x - offset.x;
            float newY = points[i].y - offset.y;
            Vector2 offsetedPoint = new(newX, newY);
            output[i] = offsetedPoint;
        }
        return output;
    }
    private static (Vector2 leftMost, Vector2 rightMost, Vector2 upMost, Vector2 downMost) GetBoundPoints(Vector2[] collection)
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
    #region Debug Methods
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
    [MenuItem("Debug/Spell System/TestPointOffseter")]
    private static void Debug_TestPointCenterer()
    {
        int pointsToGenerate = 5;
        float desiredCenterX = Random.Range(-20f, 20f);
        float desiredCenterY = Random.Range(-20f, 20f);
        Vector2 center = new(desiredCenterX, desiredCenterY);
        Vector2[] pointsToOffset = new Vector2[pointsToGenerate];
        for (int i = 0; i < pointsToOffset.Length; i++)
        {
            float x = Random.Range(-20f, 20f);
            float y = Random.Range(-20f, 20f);
            pointsToOffset[i] = new(x, y);
        }
        Vector2[] output = CenterPointGrouping(pointsToOffset, center);
        Debug.Log($"Test Input: Generate {pointsToGenerate} points, Center around {desiredCenterX}, {desiredCenterY}. Points: {pointsToOffset.ToCommaSeparatedString()} \n Test Output: {output.ToCommaSeparatedString()}");
    }
    #endif
    #endregion
}
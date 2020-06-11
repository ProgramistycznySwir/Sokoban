using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CameraTest
    {
        
        [Test]
        public void CameraPositionTesting()
        {
            // Inicjowanie obiektów i ustawienie wartości oczekiwanej
            Vector2Int mapSize = new Vector2Int(10,10);
            Level level = new Level();
            Vector3 expected = new Vector3(5f, 10.3923054f, -5f);
            
            // Wywołanie sprawdzanej metody
            Vector3 actual = level.PlaceCamera(mapSize);
            
            // Porównywanie wartości oczekiwanych z otrzymanymi
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected[2], actual[2]);

        }

    }
}

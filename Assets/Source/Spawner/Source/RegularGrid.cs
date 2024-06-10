using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battlemage.Spawner
{
	internal class Cell : IEnumerable<Transform>
	{
		private readonly HashSet<Transform> points = new HashSet<Transform>();

		public Vector2Int Index { get; }



		public Cell(Vector2Int index)
		{
			Index = index;
		}

		public void Add(Transform point)
		{
			points.Add(point);
		}

		public IEnumerator<Transform> GetEnumerator()
		{
			return points.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return points.GetEnumerator();
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode();
		}
	}

	internal class RegularGrid
	{
		private readonly int _cellSize;

		private readonly Dictionary<Vector2Int, Cell> _cells = new Dictionary<Vector2Int, Cell>();

		private int _maxRowIndex = int.MinValue;

		private int _maxColumnIndex = int.MinValue;

		private int _minRowIndex = int.MaxValue;

		private int _minColumnIndex = int.MaxValue;



		public RegularGrid(float radius, Transform[] points)
		{
			_cellSize = Mathf.CeilToInt(radius);

			BuildGrid(points);
		}

		private void BuildGrid(Transform[] points)
		{
			foreach (var transform in points)
			{
				var point = transform.position;

				Vector2Int cellIndex = GetQuad(point, _cellSize);

				if (!_cells.TryGetValue(cellIndex, out var cell))
				{
					cell = CreateCell(cellIndex);					
				}

				cell.Add(transform);
			}
		}

		private Cell CreateCell(Vector2Int cellIndex)
		{
			var cell = new Cell(cellIndex);

			if (cellIndex.x > _maxColumnIndex)
				_maxColumnIndex = cellIndex.x;

			if (cellIndex.y > _maxRowIndex)
				_maxRowIndex = cellIndex.y;

			if (cellIndex.x < _minColumnIndex)
				_minColumnIndex = cellIndex.x;

			if (cellIndex.y < _minRowIndex)
				_minRowIndex = cellIndex.y;

			_cells.Add(cellIndex, cell);

			return cell;
		}

		private bool CheckBounds(Vector2Int index)
		{
			return 
				   index.x <= _maxColumnIndex
				&& index.x >= _minColumnIndex
				&& index.y <= _maxRowIndex
				&& index.y >= _minRowIndex;
		}

		public List<Transform> GetNearest(Vector3 targetPoint, int minCount, float excludeRadius)
		{
			var targetIndex = GetQuad(targetPoint, _cellSize);

			if (!CheckBounds(targetIndex))
				throw new InvalidOperationException("Target point is out of bounds of the grid");

			List<Transform> nearest = new List<Transform>();

			var sqrRadius = excludeRadius * excludeRadius;

			var deep = 1;

			//TODO

			return nearest;
		}

		private bool CheckDistance(Vector3 first, Vector3 second,float sqrExcludeDistance)
		{
			return (first - second).sqrMagnitude >= sqrExcludeDistance;
		}

		internal void DrawGizmos()
		{
			foreach (var cell in _cells.Keys)
			{
				float left = cell.x * _cellSize;
				float right = cell.x * _cellSize + _cellSize;
				float top = cell.y * _cellSize + _cellSize;
				float bottom = cell.y * _cellSize;

				Gizmos.DrawSphere(new Vector3(cell.x * _cellSize, 0f, cell.y * _cellSize), 0.3f);
				Gizmos.DrawLine(new Vector3(left, 0f, top), new Vector3(left, 0f, bottom));
				Gizmos.DrawLine(new Vector3(right, 0f, top), new Vector3(right, 0f, bottom));
				Gizmos.DrawLine(new Vector3(left, 0f, top), new Vector3(right, 0f, top));
				Gizmos.DrawLine(new Vector3(left, 0f, bottom), new Vector3(right, 0f, bottom));
			}
		}

		/// <summary>
		/// Определяет квад по координатам точки
		/// </summary>
		/// <param name="point">Точка в кваде</param>
		public static Vector2Int GetQuad(Vector3 point, int cellSize)
			=> new Vector2Int(GetQuadComponent(point.x, cellSize), GetQuadComponent(point.z, cellSize));

		/// <summary>
		/// Преобразует один из компонентов координаты точки в компонент координаты квада
		/// </summary>
		/// <param name="coord">Компонент координаты</param>
		public static int GetQuadComponent(float coord, int cellSize)
			=> Mathf.FloorToInt(coord / cellSize);
	}
}

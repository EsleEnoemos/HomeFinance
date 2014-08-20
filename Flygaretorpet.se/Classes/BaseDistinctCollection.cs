using System;
using System.Collections;
using System.Collections.Generic;

namespace Flygaretorpet.se.Classes {
	public abstract class BaseDistinctCollection<T> : IEnumerable<T> where T : class, IUniqueID {
		private List<T> list = new List<T>();
		private CB cb;
		private Hashtable idHash;
		#region public int Count
		/// <summary>
		/// Returns the number of items added to this list
		/// </summary>
		/// <value></value>
		public int Count {
			get {
				return list.Count;
			}
		}
		#endregion
		public virtual T this[ int index ] {
			get {
				return list[ index ];
			}
			protected set {
				list[ index ] = value;
			}
		}

		protected void Sort( Comparison<T> comparer ) {
			list.Sort( comparer );
			cb = null;
		}
		protected void Sort( IComparer<T> comparer ) {
			list.Sort( comparer );
			cb = null;
		}
		protected BaseDistinctCollection() {
		}
		protected BaseDistinctCollection(IEnumerable<T> items) {
			Init( items );
		}
		private void Init( IEnumerable<T> items ) {
			AddDistinctRange( items );
		}
		public bool Contains( T item ) {
			return item != null && Contains( item.ID );
		}
		public bool Contains( int id ) {
			return idHash != null && idHash.ContainsKey( id );
		}
		public bool Contains( BaseDistinctCollection<T> these ) {
			if( these == null ) {
				return false;
			}
			foreach(T that in these) {
				if( Contains( that ) ) {
					return true;
				}
			}
			return false;
		}
		public T GetByID( int id ) {
			if( idHash == null ) {
				return null;
			}
			return (T)idHash[ id ];
		}
		protected void Add( T item ) {
			list.Add( item );
			cb = null;
		}
		public virtual void AddDistinct( T item ) {
			if( item == null || item.ID == 0 ) {
				return;
			} 
			if( idHash == null ) {
				idHash = new Hashtable();
			}
			if( idHash.ContainsKey( item.ID ) ) {
				return;
			}
			idHash[ item.ID ] = item;
			Add( item );
			cb = null;
		}
		public virtual void AddDistinctRange( IEnumerable<T> items ) {
			if( items != null ) {
				foreach(T item in items) {
					AddDistinct( item );
				}
			}
		}
		public virtual void Remove( T item ) {
			if( item == null ) {
				return;
			}
			if( list.Contains( item ) ) {
				list.Remove( item );
			}
			if( idHash != null && idHash.ContainsKey( item.ID ) ) {
				idHash.Remove( item.ID );
			}
			cb = null;
		}
		public virtual void RemoveAt( int index ) {
			list.RemoveAt( index );
		}
		public T[] ToArray() {
			return list.ToArray();
		}
		public IEnumerator<T> GetEnumerator() {
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return list.GetEnumerator();
		}

		public CollectionBase ToCollectionBase() {
			if( cb == null ) {
				cb = new CB();
				foreach(T t in list) {
					cb.Add( t );
				}
			}
			return cb;
		}
		private class CB : CollectionBase {
			public T this[ int index ] {
				get {
					return (T)List[ index ];
				}
			}
			public void Add( T that ) {
				List.Add( that );
			}
		}
	}
}
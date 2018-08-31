using System;
using Android.Support.V7.Widget;

namespace LucidX.Droid.Source.Adapters
{
	public class InfiniteScrollListener : RecyclerView.OnScrollListener
	{
		/// <summary>
		/// How many items away from the end of the list before we need to
		/// trigger a load of the next page of items
		/// </summary>

		LinearLayoutManager _linearLayoutManager;
		Action LoadMore;

		private int previousTotal = 0;
		private bool loading = true;
		int firstVisibleItem, visibleItemCount, totalItemCount;

		public InfiniteScrollListener(LinearLayoutManager linearLayoutManager,
								  Action moreItemsLoadedCallback)
		{
			_linearLayoutManager = linearLayoutManager;
			LoadMore = moreItemsLoadedCallback;
		}

		public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
		{
			base.OnScrolled(recyclerView, dx, dy);

			visibleItemCount = recyclerView.ChildCount;
			totalItemCount = _linearLayoutManager.ItemCount;
			firstVisibleItem = _linearLayoutManager.FindFirstVisibleItemPosition();

			if (loading)
			{
				if (totalItemCount > previousTotal)
				{
					loading = false;
					previousTotal = totalItemCount;
				}
			}

			if (!loading && (totalItemCount - visibleItemCount) <= firstVisibleItem)
			{
				// End has been reached
				LoadMore();
				loading = true;
			}
		}
	}
}


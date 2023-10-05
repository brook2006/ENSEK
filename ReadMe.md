# Issues Noted, But Not Raised
-	[For Candidate Testing Purposes Only] being on every page
-	The time displayed on the Buy Energy page is intentionally GMT and not the customer’s local time.
-	The “Back to Homepage” link on the Buy Energy page is unnecessary and inconsistent with other pages, but since the Sell Energy page isn’t accessible to compare, this might be correct.
-	Buy Energy page elements alignment isn’t great, but in the interests of time and the fact I have already raised an alignment issue I am skipping raising these.
-	All of the page titles have a full stop at the end, except the Buy Energy page. Similar to the above point on alignment, I didn’t raise this to save time for more interesting testing.
-	On the Buy Energy page we shouldn’t be presenting a unit price for energy we don’t have available to sell. Prices are variable and if this price is cheap it will frustrate customers that it isn’t available and if it is expensive, it will deter return business. Suggest replacing with an enquiry link.
-	The Buy Energy page has no purchase facility, no invoicing, no purchase total or purchase confirmation. Also I am effecting publicly available units for other customers without being logged in.
-	The fields only accept int32 values which means very large numbers will break the fields. This is the same kind of validation issue as already raised in bug 9.

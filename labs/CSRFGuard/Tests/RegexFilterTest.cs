using System;
using System.IO;
using NUnit.Framework;
using System.Text;  

namespace org.owasp.csrfguard.ResponseFilters.Tests
{
	/// <summary>
	/// Summary description for CSRFGuardTests.
	/// </summary>
	[TestFixture]
	public class CSRFGuardTests
	{

        private const String tokenName = "OWASP_CSRFTOKEN";
        private const String tokenValue = "64967d8f594a99dd531c2785226327b9";

		public CSRFGuardTests()
		{
			// No-op
		}

        /********************************************************************************
         * Positive tests
         *******************************************************************************/

        // Rewrite </form> end tag
        [Test]
        public void RewriteFORMTag()
        {
            String testString = "</form>" + "      </html>";

            Byte[] bytes = UTF8Encoding.UTF8.GetBytes(testString);
            MemoryStream stream = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);

            RegExFilter testFilter = new RegExFilter(stream, tokenName, tokenValue);
            testFilter.Write(bytes, 0, bytes.Length);

            String responseHtml = testFilter.getResponseHTML.ToString();
            
            // strings must not be the same, else that means no filtering happened
            Assert.IsFalse( testString == responseHtml,
               "responseHtml was not modified by the filter.  It is still:  {0}",
               responseHtml );
        }

        // Rewrite HREF <a href="">blah</a>
		[Test]
        public void RewriteNiceHREF()
		{
            String testString = "<a href=\"/something/blah.aspx\">Link Name</a>" + "      </html>";

            Byte[] bytes = UTF8Encoding.UTF8.GetBytes(testString);
            MemoryStream stream = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);

            RegExFilter testFilter = new RegExFilter(stream, tokenName, tokenValue);
            testFilter.Write(bytes, 0, bytes.Length);

            String responseHtml = testFilter.getResponseHTML.ToString();

            Assert.IsFalse(testString == responseHtml,
               "responseHtml was not modified by the filter.  It is still:  {0}",
               responseHtml);
		}

        // Rewrite HREF with inconsistent spaces
        // <a href   =  blah>
        [Test]
        public void RewriteHREFWithInconsistentSpaces()
        {
            String testString = "<a  href    =   \"/something/blah.aspx\">Link Name</a>" + "      </html>";

            Byte[] bytes = UTF8Encoding.UTF8.GetBytes(testString);
            MemoryStream stream = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);

            RegExFilter testFilter = new RegExFilter(stream, tokenName, tokenValue);
            testFilter.Write(bytes, 0, bytes.Length);

            String responseHtml = testFilter.getResponseHTML.ToString();

            Assert.IsFalse(testString == responseHtml,
               "responseHtml was not modified by the filter.  It is still:  {0}",
               responseHtml);
        }

        // Rewrite HREF with additional attributes

        // Rewrite HREF without double-quotes around values

        // Rewrite HREF with single-quotes

        // Test case insensitivity

        // Test with relative URL starting with /

        // Test with relative URL without a starting /

        // Test with full URL matching this server

        // Test with url matching .(gif|jpg|png|css|js|ico|swf|axd|pdf)$ and not

        /********************************************************************************
         * Negative tests
         *******************************************************************************/

        // Ignore javascript URLs

		// Leave non-HREF tag alone <a hat="wintery">
	}
}
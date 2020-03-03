# CryptoRates

<h2>How to enable Google authentication:</h2>

<ul>
<li>Navigate to <a href="https://developers.google.com/identity/sign-in/web/devconsole-project" data-linktype="external">Integrating Google Sign-In into your web app</a> and select <strong>CONFIGURE A PROJECT</strong>.</li>
<li>In the <strong>Configure your OAuth client</strong> dialog, select <strong>Web server</strong>.</li>
<li>In the <strong>Authorized redirect URIs</strong> text entry box, set the redirect URI: <code>https://localhost:5001/signin-google</code></li>
<li>Save the <strong>Client ID</strong> and <strong>Client Secret</strong> in <strong>"Authentication:Google"</strong> section
  of <strong>appsettings.json</strong>.</li>
<li>When deploying the site, register the new public url from the <strong>Google Console</strong>.</li>
</ul>

<h2>How to enable Email notifications:</h2>

<ol>
  <li>Go to your <a href="https://myaccount.google.com/" target="_blank" rel="noopener">Google Account</a>.</li>
  <li>On the left navigation panel, choose <strong>Security</strong>.</li>
  <li>On the "Signing in to Google" panel, choose <strong>App Passwords</strong>. If you don’t see this option:
    <ul>
      <li>2-Step Verification is not set up for your account.</li>
      <li>2-Step Verification is set up for security keys only.</li>
      <li>Your account is through work, school, or other organization.</li>
      <li>You’ve turned on Advanced Protection for your account.</li>
    </ul>
  </li>
  <li>At the bottom, choose <strong>Select app </strong> and write "CryptoRates" (or anything else)</li>
  <li>Choose <strong>Generate</strong>.</li>
  <li>he App Password is the 16-character code in the yellow bar on your device. Save it</li>
  <li>Write your <strong>Google username</strong> and <strong>App password</strong> in <strong>"SmtpClient"</strong> section
  of <strong>appsettings.json</strong>.</li>
</ol>

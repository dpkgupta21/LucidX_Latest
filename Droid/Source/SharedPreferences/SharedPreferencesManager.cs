

using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Javax.Crypto;
using Java.Lang;
using Java.Security;
using Java.IO;
using Javax.Crypto.Spec;
using Android.Util;
using LucidX.ResponseModels;
using Newtonsoft.Json;
using LucidX.Droid.Source.Global;

namespace LucidX.Droid.Source.SharedPreference
{

    /// <summary>
    /// Shared Preference Manager for providing facility to store preference values in encrypted form or in simple form
    /// </summary>
    public class SharedPreferencesManager
    {
        private Context _context;
        private ISharedPreferences _preferences;
        private ISharedPreferencesEditor _editor;

        private static string TRANSFORMATION = "AES/CBC/PKCS5Padding";
        private static string KEY_TRANSFORMATION = "AES/ECB/PKCS5Padding";
        private static string SECRET_KEY_HASH_TRANSFORMATION = "SHA-256";

        private bool _encryptKeys;
        private bool _encryptValues;
        private Cipher _writer;
        private Cipher _reader;
        private Cipher _keyWriter;


        /// <summary>
        /// Shared Preferences Manager Custom Exception Class
        /// </summary>
        public class SharedPreferencesManagerException : RuntimeException
        {

            public SharedPreferencesManagerException(Throwable e) : base(e)
            {
            }
        }

        /// <summary>
        /// Preference Manager constructor - Get SharedPreferencesManager with given settings for name, mode and encryption in arguments
        /// </summary>
        /// <param name="context"></param>
        /// <param name="preferenceName"></param>
        /// <param name="mode"></param>
        /// <param name="secureKey"></param>
        /// <param name="encryptKeys"></param>
        /// <param name="encryptValues"></param>
        public SharedPreferencesManager(Context context, string preferenceName,
                                  FileCreationMode mode, string secureKey,
                                  bool encryptKeys, bool encryptValues)
        {
            try
            {
                _context = context;

                this._writer = Cipher.GetInstance(TRANSFORMATION);
                this._reader = Cipher.GetInstance(TRANSFORMATION);
                this._keyWriter = Cipher.GetInstance(KEY_TRANSFORMATION);
                this._encryptKeys = encryptKeys;
                this._encryptValues = encryptValues;

                InitCiphers(secureKey);

                _preferences = _context.GetSharedPreferences(preferenceName, mode);
                _editor = _preferences.Edit();

            }
            catch (GeneralSecurityException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
        }


        /// <summary>
        /// Preference Manager constructor- Preference Manager constructor - Get SharedPreferencesManager with given settings for name and encryption in arguments and Preference mode as MODE_PRIVATE by default
        /// </summary>
        /// <param name="context"></param>
        /// <param name="preferenceName"></param>
        /// <param name="secureKey"></param>
        /// <param name="encryptKeys"></param>
        /// <param name="encryptValues"></param>
        public SharedPreferencesManager(Context context, string preferenceName, string secureKey,
                                        bool encryptKeys, bool encryptValues)
        {
            try
            {
                _context = context;

                this._writer = Cipher.GetInstance(TRANSFORMATION);
                this._reader = Cipher.GetInstance(TRANSFORMATION);
                this._keyWriter = Cipher.GetInstance(KEY_TRANSFORMATION);
                this._encryptKeys = encryptKeys;
                this._encryptValues = encryptValues;

                InitCiphers(secureKey);

                _preferences = _context.GetSharedPreferences(preferenceName, FileCreationMode.Private);
                _editor = _preferences.Edit();

            }
            catch (GeneralSecurityException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
        }


        /// <summary>
        /// Preference Manager constructor - Get SharedPreferencesManager with encryption disabled
        /// </summary>
        /// <param name="context"></param>
        /// <param name="preferenceName"></param>
        /// <param name="mode"></param>
        public SharedPreferencesManager(Context context, string preferenceName, FileCreationMode mode)
        {
            try
            {
                _context = context;

                this._writer = Cipher.GetInstance(TRANSFORMATION);
                this._reader = Cipher.GetInstance(TRANSFORMATION);
                this._keyWriter = Cipher.GetInstance(KEY_TRANSFORMATION);
                this._encryptKeys = false;
                this._encryptValues = false;

                InitCiphers("");

                _preferences = _context.GetSharedPreferences(preferenceName, mode);
                _editor = _preferences.Edit();

            }
            catch (GeneralSecurityException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
        }


        /// <summary>
        ///  Preference Manager constructor - Get SharedPreferencesManager with encryption disabled and Preference mode as MODE_PRIVATE by default
        /// </summary>
        /// <param name="context"></param>
        /// <param name="preferenceName"></param>
        public SharedPreferencesManager(Context context, string preferenceName)
        {
            try
            {
                _context = context;

                this._writer = Cipher.GetInstance(TRANSFORMATION);
                this._reader = Cipher.GetInstance(TRANSFORMATION);
                this._keyWriter = Cipher.GetInstance(KEY_TRANSFORMATION);
                this._encryptKeys = false;
                this._encryptValues = false;

                InitCiphers("");

                _preferences = _context.GetSharedPreferences(preferenceName, FileCreationMode.Private);
                _editor = _preferences.Edit();

            }
            catch (GeneralSecurityException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
        }




        /// <summary>
        /// Initialize cipher for Encryption and Decription
        /// </summary>
        /// <param name="secureKey"></param>
        protected void InitCiphers(string secureKey)
        {
            try
            {
                IvParameterSpec ivSpec = GetIv();
                SecretKeySpec secretKey = GetSecretKey(secureKey);

                _writer.Init(Cipher.EncryptMode, secretKey, ivSpec);
                _reader.Init(Cipher.DecryptMode, secretKey, ivSpec);
                _keyWriter.Init(Cipher.EncryptMode, secretKey);
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
            catch (NoSuchAlgorithmException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
            catch (InvalidKeyException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
            catch (InvalidAlgorithmParameterException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
        }


        /// <summary>
        /// Create and Get IV parameter Specs
        /// </summary>
        /// <returns></returns>
        protected IvParameterSpec GetIv()
        {
            byte[] iv = new byte[_writer.BlockSize];

            byte[] bytes = Encoding.ASCII.GetBytes("fldsjfodasjifudslfjdsaofshaufihadsf");

            Array.Copy(bytes, 0, iv, 0, _writer.BlockSize);
            return new IvParameterSpec(iv);
        }


        /// <summary>
        /// Create Secret key Specs
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected SecretKeySpec GetSecretKey(string key)
        {
            try
            {

                byte[] keyBytes = CreateKeyBytes(key);
                return new SecretKeySpec(keyBytes, TRANSFORMATION);

            }
            catch (UnsupportedEncodingException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
            catch (NoSuchAlgorithmException e)
            {
                throw new SharedPreferencesManagerException(e);
            }

        }

        protected byte[] CreateKeyBytes(string key)
        {
            try
            {

                MessageDigest md = MessageDigest.GetInstance(SECRET_KEY_HASH_TRANSFORMATION);
                md.Reset();
                byte[] keyBytes = md.Digest(Encoding.UTF8.GetBytes(key)/*key.GetBytes(CHARSET)*/);
                return keyBytes;
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
            catch (NoSuchAlgorithmException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
        }

        /// <summary>
        /// Encrypt key if encryption enable for Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string ToKey(string key)
        {
            if (_encryptKeys)
            {
                return Encrypt(key, _keyWriter);
            }
            else
            {
                return key;
            }
        }

        /// <summary>
        ///  Encrypt value if encryption enable for Value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ToValue(string value)
        {
            if (_encryptValues)
            {
                return Encrypt(value, _writer);
            }

            else
            {
                return value;
            }
        }

        /// <summary>
        ///  Encrypt given string using AES Encryption
        /// </summary>
        /// <param name="value"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected string Encrypt(string value, Cipher writer)
        {

            byte[] secureValue;
            try
            {
                secureValue = Convert(writer, Encoding.UTF8.GetBytes(value)/* value.getBytes(CHARSET)*/);
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
            string secureValueEncoded = Base64.EncodeToString(secureValue, Base64.NoWrap);
            return secureValueEncoded;
        }

        /**
         * @param securedEncodedValue
         * @return Decrypted String
         */
        protected string Decrypt(string securedEncodedValue)
        {
            byte[] securedValue = Base64.Decode(securedEncodedValue, Base64.NoWrap);
            byte[] value = Convert(_reader, securedValue);
            try
            {
                return Encoding.UTF8.GetString(value);/*new string(value, CHARSET)*/
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SharedPreferencesManagerException(e);
            }
        }

        private static byte[] Convert(Cipher cipher, byte[] bs)
        {
            try
            {
                return cipher.DoFinal(bs);
            }
            catch (System.Exception e)
            {
                return null;
            }
        }


        /// <summary>
        ///  Set a String value in the mPreferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutString(string key, string value)
        {
            if (value == null)
            {
                _editor.Remove(ToKey(key)).Commit();
            }
            else {
                string secureValueEncoded = ToValue(value);
                _editor.PutString(ToKey(key), secureValueEncoded).Apply();
            }
        }

        /// <summary>
        /// Set a Boolean value in the mPreferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutBoolean(string key, bool value)
        {
            _editor.PutBoolean(ToKey(key), value);
            _editor.Apply();
        }

        /// <summary>
        /// Set a Float value in the mPreferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutFloat(string key, float value)
        {
            _editor.PutFloat(ToKey(key), value);
            _editor.Apply();
        }

        /// <summary>
        /// Set a Integer value in the mPreferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutInt(string key, int value)
        {
            _editor.PutInt(ToKey(key), value);
            _editor.Apply();
        }

        /// <summary>
        ///  Set a Long value in the mPreferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutLong(string key, long value)
        {
            _editor.PutLong(ToKey(key), value);
            _editor.Apply();
        }


        /// <summary>
        ///  Retrieve a set of String values from the mPreferences.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue)
        {
            if (_preferences.Contains(ToKey(key)))
            {
                string securedEncodedValue = _preferences.GetString(ToKey(key), defaultValue);
                if (_encryptValues)
                { // If encryption is enabled for values then send decrypted value
                    return Decrypt(securedEncodedValue);
                }
                else {
                    return securedEncodedValue;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  Retrieve a boolean value from the mPreferences.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetBoolean(string key, bool defaultValue)
        {
            if (_preferences.Contains(ToKey(key)))
            {
                return _preferences.GetBoolean(ToKey(key), defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        ///  Retrieve an int value from the mPreferences.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetInt(string key, int defaultValue)
        {
            if (_preferences.Contains(ToKey(key)))
            {
                return _preferences.GetInt(ToKey(key), defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        ///  Retrieve an Float value from the mPreferences.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float GetFloat(string key, float defaultValue)
        {
            if (_preferences.Contains(ToKey(key)))
            {
                return _preferences.GetFloat(ToKey(key), defaultValue);
            }
            return defaultValue;

        }

        /// <summary>
        /// Retrieve an Long value from the mPreferences.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long GetLong(string key, long defaultValue)
        {
            if (_preferences.Contains(ToKey(key)))
            {
                return _preferences.GetLong(ToKey(key), defaultValue);
            }
            return defaultValue;

        }


        /// <summary>
        /// Retrieve all values from the mPreferences.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetAll()
        {
            return _preferences.All;
        }





        /// <summary>
        ///  Remove all values from mPreferences
        /// </summary>
        public void ClearAllPreferencesData()
        {
            _editor.Clear().Commit();
        }

        /// <summary>
        /// Remove a preference value from mPreferences
        /// </summary>
        /// <param name="key"></param>
        public void RemoveValue(string key)
        {
            _editor.Remove(ToKey(key)).Commit();
        }

        /// <summary>
        /// Checks whether the mPreferences contains a preference.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _preferences.Contains(ToKey(key));
        }

        /// <summary>
        ///  Set a Email Count Response value in the mPreferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutCountResponse(EmailCountResponse countObj)
        {
            string key = ConstantsDroid.EMAIL_COUNT_OBJECT_PREFERENCE;
            string value = JsonConvert.SerializeObject(countObj);           
            if (value == null)
            {
                _editor.Remove(ToKey(key)).Commit();
            }
            else
            {
                string secureValueEncoded = ToValue(value);
                _editor.PutString(ToKey(key), secureValueEncoded).Apply();
            }
        }

        /// <summary>
        ///  Set a Email Count Response value in the mPreferences
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutLoginResponse(LoginResponse loginObj)
        {
            string key = ConstantsDroid.LOGIN_RESPONSE_OBJECT_PREFERENCE;
            string value = JsonConvert.SerializeObject(loginObj);
            if (value == null)
            {
                _editor.Remove(ToKey(key)).Commit();
            }
            else
            {
                string secureValueEncoded = ToValue(value);
                _editor.PutString(ToKey(key), secureValueEncoded).Apply();
            }
        }

        /// <summary>
        ///  Retrieve a Count Object from the mPreferences.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public EmailCountResponse GetCountResponse()
        {
            string key = ConstantsDroid.EMAIL_COUNT_OBJECT_PREFERENCE;
            EmailCountResponse response = null;
            string value = null;
            try
            {
               
                if (_preferences.Contains(ToKey(key)))
                {
                    string securedEncodedValue = _preferences.GetString(ToKey(key), "");
                    if (_encryptValues)
                    { // If encryption is enabled for values then send decrypted value
                        value = Decrypt(securedEncodedValue);
                        response = JsonConvert.DeserializeObject<EmailCountResponse>(value);
                        return response;
                    }
                    else
                    {
                        value = securedEncodedValue;
                        response = JsonConvert.DeserializeObject<EmailCountResponse>(value);
                        return response;
                    }
                }
            }catch(System.Exception e)
            {
                return response;
            }
            return response;
        }

        /// <summary>
        ///  Retrieve a Count Object from the mPreferences.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public LoginResponse GetLoginResponse()
        {
            string key = ConstantsDroid.LOGIN_RESPONSE_OBJECT_PREFERENCE;
            LoginResponse response = null;
            string value = null;
            try
            {

                if (_preferences.Contains(ToKey(key)))
                {
                    string securedEncodedValue = _preferences.GetString(ToKey(key), "");
                    if (_encryptValues)
                    { // If encryption is enabled for values then send decrypted value
                        value = Decrypt(securedEncodedValue);
                        response = JsonConvert.DeserializeObject<LoginResponse>(value);
                        return response;
                    }
                    else
                    {
                        value = securedEncodedValue;
                        response = JsonConvert.DeserializeObject<LoginResponse>(value);
                        return response;
                    }
                }
            }
            catch (System.Exception e)
            {
                return response;
            }
            return response;
        }
    }
}
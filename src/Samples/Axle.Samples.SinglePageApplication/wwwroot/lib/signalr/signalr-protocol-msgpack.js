/** 
 * @overview ASP.NET Core SignalR JavaScript Client.
 * @version 1.0.1-rtm-t000.
 * @license
 * Copyright (c) .NET Foundation. All rights reserved.
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 */
(function (global, factory) {
	typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory(require('msgpack5'), require('tslib')) :
	typeof define === 'function' && define.amd ? define(['msgpack5', 'tslib'], factory) :
	(global.signalR = global.signalR || {}, global.signalR.protocols = global.signalR.protocols || {}, global.signalR.protocols.msgpack = factory(global.msgpack5,global.tslib_1));
}(this, (function (msgpack5,tslib_1) { 'use strict';

msgpack5 = msgpack5 && msgpack5.hasOwnProperty('default') ? msgpack5['default'] : msgpack5;

function unwrapExports (x) {
	return x && x.__esModule && Object.prototype.hasOwnProperty.call(x, 'default') ? x['default'] : x;
}

function createCommonjsModule(fn, module) {
	return module = { exports: {} }, fn(module, module.exports), module.exports;
}

/*! *****************************************************************************
Copyright (c) Microsoft Corporation. All rights reserved.
Licensed under the Apache License, Version 2.0 (the "License"); you may not use
this file except in compliance with the License. You may obtain a copy of the
License at http://www.apache.org/licenses/LICENSE-2.0

THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
MERCHANTABLITY OR NON-INFRINGEMENT.

See the Apache Version 2.0 License for specific language governing permissions
and limitations under the License.
***************************************************************************** */
/* global Reflect, Promise */

var extendStatics = Object.setPrototypeOf ||
    ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
    function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };

function __extends$1(d, b) {
    extendStatics(d, b);
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
}

var __assign$1 = Object.assign || function __assign$$1(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p)) t[p] = s[p];
    }
    return t;
};

function __rest(s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
}

function __decorate(decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
}

function __param(paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
}

function __metadata(metadataKey, metadataValue) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(metadataKey, metadataValue);
}

function __awaiter$1(thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
}

function __generator$1(thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
}

function __exportStar(m, exports) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}

function __values(o) {
    var m = typeof Symbol === "function" && o[Symbol.iterator], i = 0;
    if (m) return m.call(o);
    return {
        next: function () {
            if (o && i >= o.length) o = void 0;
            return { value: o && o[i++], done: !o };
        }
    };
}

function __read(o, n) {
    var m = typeof Symbol === "function" && o[Symbol.iterator];
    if (!m) return o;
    var i = m.call(o), r, ar = [], e;
    try {
        while ((n === void 0 || n-- > 0) && !(r = i.next()).done) ar.push(r.value);
    }
    catch (error) { e = { error: error }; }
    finally {
        try {
            if (r && !r.done && (m = i["return"])) m.call(i);
        }
        finally { if (e) throw e.error; }
    }
    return ar;
}

function __spread() {
    for (var ar = [], i = 0; i < arguments.length; i++)
        ar = ar.concat(__read(arguments[i]));
    return ar;
}

function __await(v) {
    return this instanceof __await ? (this.v = v, this) : new __await(v);
}

function __asyncGenerator(thisArg, _arguments, generator) {
    if (!Symbol.asyncIterator) throw new TypeError("Symbol.asyncIterator is not defined.");
    var g = generator.apply(thisArg, _arguments || []), i, q = [];
    return i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i;
    function verb(n) { if (g[n]) i[n] = function (v) { return new Promise(function (a, b) { q.push([n, v, a, b]) > 1 || resume(n, v); }); }; }
    function resume(n, v) { try { step(g[n](v)); } catch (e) { settle(q[0][3], e); } }
    function step(r) { r.value instanceof __await ? Promise.resolve(r.value.v).then(fulfill, reject) : settle(q[0][2], r);  }
    function fulfill(value) { resume("next", value); }
    function reject(value) { resume("throw", value); }
    function settle(f, v) { if (f(v), q.shift(), q.length) resume(q[0][0], q[0][1]); }
}

function __asyncDelegator(o) {
    var i, p;
    return i = {}, verb("next"), verb("throw", function (e) { throw e; }), verb("return"), i[Symbol.iterator] = function () { return this; }, i;
    function verb(n, f) { if (o[n]) i[n] = function (v) { return (p = !p) ? { value: __await(o[n](v)), done: n === "return" } : f ? f(v) : v; }; }
}

function __asyncValues(o) {
    if (!Symbol.asyncIterator) throw new TypeError("Symbol.asyncIterator is not defined.");
    var m = o[Symbol.asyncIterator];
    return m ? m.call(o) : typeof __values === "function" ? __values(o) : o[Symbol.iterator]();
}

function __makeTemplateObject(cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
}

function __importStar(mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) result[k] = mod[k];
    result.default = mod;
    return result;
}

function __importDefault(mod) {
    return (mod && mod.__esModule) ? mod : { default: mod };
}


var tslib_es6 = Object.freeze({
	__extends: __extends$1,
	__assign: __assign$1,
	__rest: __rest,
	__decorate: __decorate,
	__param: __param,
	__metadata: __metadata,
	__awaiter: __awaiter$1,
	__generator: __generator$1,
	__exportStar: __exportStar,
	__values: __values,
	__read: __read,
	__spread: __spread,
	__await: __await,
	__asyncGenerator: __asyncGenerator,
	__asyncDelegator: __asyncDelegator,
	__asyncValues: __asyncValues,
	__makeTemplateObject: __makeTemplateObject,
	__importStar: __importStar,
	__importDefault: __importDefault
});

var byteLength_1 = byteLength;
var toByteArray_1 = toByteArray;
var fromByteArray_1 = fromByteArray;

var lookup = [];
var revLookup = [];
var Arr = typeof Uint8Array !== 'undefined' ? Uint8Array : Array;

var code = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/';
for (var i = 0, len = code.length; i < len; ++i) {
  lookup[i] = code[i];
  revLookup[code.charCodeAt(i)] = i;
}

revLookup['-'.charCodeAt(0)] = 62;
revLookup['_'.charCodeAt(0)] = 63;

function placeHoldersCount (b64) {
  var len = b64.length;
  if (len % 4 > 0) {
    throw new Error('Invalid string. Length must be a multiple of 4')
  }

  // the number of equal signs (place holders)
  // if there are two placeholders, than the two characters before it
  // represent one byte
  // if there is only one, then the three characters before it represent 2 bytes
  // this is just a cheap hack to not do indexOf twice
  return b64[len - 2] === '=' ? 2 : b64[len - 1] === '=' ? 1 : 0
}

function byteLength (b64) {
  // base64 is 4/3 + up to two characters of the original data
  return (b64.length * 3 / 4) - placeHoldersCount(b64)
}

function toByteArray (b64) {
  var i, l, tmp, placeHolders, arr;
  var len = b64.length;
  placeHolders = placeHoldersCount(b64);

  arr = new Arr((len * 3 / 4) - placeHolders);

  // if there are placeholders, only get up to the last complete 4 chars
  l = placeHolders > 0 ? len - 4 : len;

  var L = 0;

  for (i = 0; i < l; i += 4) {
    tmp = (revLookup[b64.charCodeAt(i)] << 18) | (revLookup[b64.charCodeAt(i + 1)] << 12) | (revLookup[b64.charCodeAt(i + 2)] << 6) | revLookup[b64.charCodeAt(i + 3)];
    arr[L++] = (tmp >> 16) & 0xFF;
    arr[L++] = (tmp >> 8) & 0xFF;
    arr[L++] = tmp & 0xFF;
  }

  if (placeHolders === 2) {
    tmp = (revLookup[b64.charCodeAt(i)] << 2) | (revLookup[b64.charCodeAt(i + 1)] >> 4);
    arr[L++] = tmp & 0xFF;
  } else if (placeHolders === 1) {
    tmp = (revLookup[b64.charCodeAt(i)] << 10) | (revLookup[b64.charCodeAt(i + 1)] << 4) | (revLookup[b64.charCodeAt(i + 2)] >> 2);
    arr[L++] = (tmp >> 8) & 0xFF;
    arr[L++] = tmp & 0xFF;
  }

  return arr
}

function tripletToBase64 (num) {
  return lookup[num >> 18 & 0x3F] + lookup[num >> 12 & 0x3F] + lookup[num >> 6 & 0x3F] + lookup[num & 0x3F]
}

function encodeChunk (uint8, start, end) {
  var tmp;
  var output = [];
  for (var i = start; i < end; i += 3) {
    tmp = (uint8[i] << 16) + (uint8[i + 1] << 8) + (uint8[i + 2]);
    output.push(tripletToBase64(tmp));
  }
  return output.join('')
}

function fromByteArray (uint8) {
  var tmp;
  var len = uint8.length;
  var extraBytes = len % 3; // if we have 1 byte left, pad 2 bytes
  var output = '';
  var parts = [];
  var maxChunkLength = 16383; // must be multiple of 3

  // go through the array every three bytes, we'll deal with trailing stuff later
  for (var i = 0, len2 = len - extraBytes; i < len2; i += maxChunkLength) {
    parts.push(encodeChunk(uint8, i, (i + maxChunkLength) > len2 ? len2 : (i + maxChunkLength)));
  }

  // pad the end with zeros, but make sure to not forget the extra bytes
  if (extraBytes === 1) {
    tmp = uint8[len - 1];
    output += lookup[tmp >> 2];
    output += lookup[(tmp << 4) & 0x3F];
    output += '==';
  } else if (extraBytes === 2) {
    tmp = (uint8[len - 2] << 8) + (uint8[len - 1]);
    output += lookup[tmp >> 10];
    output += lookup[(tmp >> 4) & 0x3F];
    output += lookup[(tmp << 2) & 0x3F];
    output += '=';
  }

  parts.push(output);

  return parts.join('')
}

var base64Js = {
	byteLength: byteLength_1,
	toByteArray: toByteArray_1,
	fromByteArray: fromByteArray_1
};

var read = function (buffer, offset, isLE, mLen, nBytes) {
  var e, m;
  var eLen = nBytes * 8 - mLen - 1;
  var eMax = (1 << eLen) - 1;
  var eBias = eMax >> 1;
  var nBits = -7;
  var i = isLE ? (nBytes - 1) : 0;
  var d = isLE ? -1 : 1;
  var s = buffer[offset + i];

  i += d;

  e = s & ((1 << (-nBits)) - 1);
  s >>= (-nBits);
  nBits += eLen;
  for (; nBits > 0; e = e * 256 + buffer[offset + i], i += d, nBits -= 8) {}

  m = e & ((1 << (-nBits)) - 1);
  e >>= (-nBits);
  nBits += mLen;
  for (; nBits > 0; m = m * 256 + buffer[offset + i], i += d, nBits -= 8) {}

  if (e === 0) {
    e = 1 - eBias;
  } else if (e === eMax) {
    return m ? NaN : ((s ? -1 : 1) * Infinity)
  } else {
    m = m + Math.pow(2, mLen);
    e = e - eBias;
  }
  return (s ? -1 : 1) * m * Math.pow(2, e - mLen)
};

var write = function (buffer, value, offset, isLE, mLen, nBytes) {
  var e, m, c;
  var eLen = nBytes * 8 - mLen - 1;
  var eMax = (1 << eLen) - 1;
  var eBias = eMax >> 1;
  var rt = (mLen === 23 ? Math.pow(2, -24) - Math.pow(2, -77) : 0);
  var i = isLE ? 0 : (nBytes - 1);
  var d = isLE ? 1 : -1;
  var s = value < 0 || (value === 0 && 1 / value < 0) ? 1 : 0;

  value = Math.abs(value);

  if (isNaN(value) || value === Infinity) {
    m = isNaN(value) ? 1 : 0;
    e = eMax;
  } else {
    e = Math.floor(Math.log(value) / Math.LN2);
    if (value * (c = Math.pow(2, -e)) < 1) {
      e--;
      c *= 2;
    }
    if (e + eBias >= 1) {
      value += rt / c;
    } else {
      value += rt * Math.pow(2, 1 - eBias);
    }
    if (value * c >= 2) {
      e++;
      c /= 2;
    }

    if (e + eBias >= eMax) {
      m = 0;
      e = eMax;
    } else if (e + eBias >= 1) {
      m = (value * c - 1) * Math.pow(2, mLen);
      e = e + eBias;
    } else {
      m = value * Math.pow(2, eBias - 1) * Math.pow(2, mLen);
      e = 0;
    }
  }

  for (; mLen >= 8; buffer[offset + i] = m & 0xff, i += d, m /= 256, mLen -= 8) {}

  e = (e << mLen) | m;
  eLen += mLen;
  for (; eLen > 0; buffer[offset + i] = e & 0xff, i += d, e /= 256, eLen -= 8) {}

  buffer[offset + i - d] |= s * 128;
};

var ieee754 = {
	read: read,
	write: write
};

var buffer = createCommonjsModule(function (module, exports) {
/*!
 * The buffer module from node.js, for the browser.
 *
 * @author   Feross Aboukhadijeh <https://feross.org>
 * @license  MIT
 */
/* eslint-disable no-proto */

exports.Buffer = Buffer;
exports.SlowBuffer = SlowBuffer;
exports.INSPECT_MAX_BYTES = 50;

var K_MAX_LENGTH = 0x7fffffff;
exports.kMaxLength = K_MAX_LENGTH;

/**
 * If `Buffer.TYPED_ARRAY_SUPPORT`:
 *   === true    Use Uint8Array implementation (fastest)
 *   === false   Print warning and recommend using `buffer` v4.x which has an Object
 *               implementation (most compatible, even IE6)
 *
 * Browsers that support typed arrays are IE 10+, Firefox 4+, Chrome 7+, Safari 5.1+,
 * Opera 11.6+, iOS 4.2+.
 *
 * We report that the browser does not support typed arrays if the are not subclassable
 * using __proto__. Firefox 4-29 lacks support for adding new properties to `Uint8Array`
 * (See: https://bugzilla.mozilla.org/show_bug.cgi?id=695438). IE 10 lacks support
 * for __proto__ and has a buggy typed array implementation.
 */
Buffer.TYPED_ARRAY_SUPPORT = typedArraySupport();

if (!Buffer.TYPED_ARRAY_SUPPORT && typeof console !== 'undefined' &&
    typeof console.error === 'function') {
  console.error(
    'This browser lacks typed array (Uint8Array) support which is required by ' +
    '`buffer` v5.x. Use `buffer` v4.x if you require old browser support.'
  );
}

function typedArraySupport () {
  // Can typed array instances can be augmented?
  try {
    var arr = new Uint8Array(1);
    arr.__proto__ = {__proto__: Uint8Array.prototype, foo: function () { return 42 }};
    return arr.foo() === 42
  } catch (e) {
    return false
  }
}

function createBuffer (length) {
  if (length > K_MAX_LENGTH) {
    throw new RangeError('Invalid typed array length')
  }
  // Return an augmented `Uint8Array` instance
  var buf = new Uint8Array(length);
  buf.__proto__ = Buffer.prototype;
  return buf
}

/**
 * The Buffer constructor returns instances of `Uint8Array` that have their
 * prototype changed to `Buffer.prototype`. Furthermore, `Buffer` is a subclass of
 * `Uint8Array`, so the returned instances will have all the node `Buffer` methods
 * and the `Uint8Array` methods. Square bracket notation works as expected -- it
 * returns a single octet.
 *
 * The `Uint8Array` prototype remains unmodified.
 */

function Buffer (arg, encodingOrOffset, length) {
  // Common case.
  if (typeof arg === 'number') {
    if (typeof encodingOrOffset === 'string') {
      throw new Error(
        'If encoding is specified then the first argument must be a string'
      )
    }
    return allocUnsafe(arg)
  }
  return from(arg, encodingOrOffset, length)
}

// Fix subarray() in ES2016. See: https://github.com/feross/buffer/pull/97
if (typeof Symbol !== 'undefined' && Symbol.species &&
    Buffer[Symbol.species] === Buffer) {
  Object.defineProperty(Buffer, Symbol.species, {
    value: null,
    configurable: true,
    enumerable: false,
    writable: false
  });
}

Buffer.poolSize = 8192; // not used by this implementation

function from (value, encodingOrOffset, length) {
  if (typeof value === 'number') {
    throw new TypeError('"value" argument must not be a number')
  }

  if (isArrayBuffer(value)) {
    return fromArrayBuffer(value, encodingOrOffset, length)
  }

  if (typeof value === 'string') {
    return fromString(value, encodingOrOffset)
  }

  return fromObject(value)
}

/**
 * Functionally equivalent to Buffer(arg, encoding) but throws a TypeError
 * if value is a number.
 * Buffer.from(str[, encoding])
 * Buffer.from(array)
 * Buffer.from(buffer)
 * Buffer.from(arrayBuffer[, byteOffset[, length]])
 **/
Buffer.from = function (value, encodingOrOffset, length) {
  return from(value, encodingOrOffset, length)
};

// Note: Change prototype *after* Buffer.from is defined to workaround Chrome bug:
// https://github.com/feross/buffer/pull/148
Buffer.prototype.__proto__ = Uint8Array.prototype;
Buffer.__proto__ = Uint8Array;

function assertSize (size) {
  if (typeof size !== 'number') {
    throw new TypeError('"size" argument must be a number')
  } else if (size < 0) {
    throw new RangeError('"size" argument must not be negative')
  }
}

function alloc (size, fill, encoding) {
  assertSize(size);
  if (size <= 0) {
    return createBuffer(size)
  }
  if (fill !== undefined) {
    // Only pay attention to encoding if it's a string. This
    // prevents accidentally sending in a number that would
    // be interpretted as a start offset.
    return typeof encoding === 'string'
      ? createBuffer(size).fill(fill, encoding)
      : createBuffer(size).fill(fill)
  }
  return createBuffer(size)
}

/**
 * Creates a new filled Buffer instance.
 * alloc(size[, fill[, encoding]])
 **/
Buffer.alloc = function (size, fill, encoding) {
  return alloc(size, fill, encoding)
};

function allocUnsafe (size) {
  assertSize(size);
  return createBuffer(size < 0 ? 0 : checked(size) | 0)
}

/**
 * Equivalent to Buffer(num), by default creates a non-zero-filled Buffer instance.
 * */
Buffer.allocUnsafe = function (size) {
  return allocUnsafe(size)
};
/**
 * Equivalent to SlowBuffer(num), by default creates a non-zero-filled Buffer instance.
 */
Buffer.allocUnsafeSlow = function (size) {
  return allocUnsafe(size)
};

function fromString (string, encoding) {
  if (typeof encoding !== 'string' || encoding === '') {
    encoding = 'utf8';
  }

  if (!Buffer.isEncoding(encoding)) {
    throw new TypeError('"encoding" must be a valid string encoding')
  }

  var length = byteLength(string, encoding) | 0;
  var buf = createBuffer(length);

  var actual = buf.write(string, encoding);

  if (actual !== length) {
    // Writing a hex string, for example, that contains invalid characters will
    // cause everything after the first invalid character to be ignored. (e.g.
    // 'abxxcd' will be treated as 'ab')
    buf = buf.slice(0, actual);
  }

  return buf
}

function fromArrayLike (array) {
  var length = array.length < 0 ? 0 : checked(array.length) | 0;
  var buf = createBuffer(length);
  for (var i = 0; i < length; i += 1) {
    buf[i] = array[i] & 255;
  }
  return buf
}

function fromArrayBuffer (array, byteOffset, length) {
  if (byteOffset < 0 || array.byteLength < byteOffset) {
    throw new RangeError('\'offset\' is out of bounds')
  }

  if (array.byteLength < byteOffset + (length || 0)) {
    throw new RangeError('\'length\' is out of bounds')
  }

  var buf;
  if (byteOffset === undefined && length === undefined) {
    buf = new Uint8Array(array);
  } else if (length === undefined) {
    buf = new Uint8Array(array, byteOffset);
  } else {
    buf = new Uint8Array(array, byteOffset, length);
  }

  // Return an augmented `Uint8Array` instance
  buf.__proto__ = Buffer.prototype;
  return buf
}

function fromObject (obj) {
  if (Buffer.isBuffer(obj)) {
    var len = checked(obj.length) | 0;
    var buf = createBuffer(len);

    if (buf.length === 0) {
      return buf
    }

    obj.copy(buf, 0, 0, len);
    return buf
  }

  if (obj) {
    if (isArrayBufferView(obj) || 'length' in obj) {
      if (typeof obj.length !== 'number' || numberIsNaN(obj.length)) {
        return createBuffer(0)
      }
      return fromArrayLike(obj)
    }

    if (obj.type === 'Buffer' && Array.isArray(obj.data)) {
      return fromArrayLike(obj.data)
    }
  }

  throw new TypeError('First argument must be a string, Buffer, ArrayBuffer, Array, or array-like object.')
}

function checked (length) {
  // Note: cannot use `length < K_MAX_LENGTH` here because that fails when
  // length is NaN (which is otherwise coerced to zero.)
  if (length >= K_MAX_LENGTH) {
    throw new RangeError('Attempt to allocate Buffer larger than maximum ' +
                         'size: 0x' + K_MAX_LENGTH.toString(16) + ' bytes')
  }
  return length | 0
}

function SlowBuffer (length) {
  if (+length != length) { // eslint-disable-line eqeqeq
    length = 0;
  }
  return Buffer.alloc(+length)
}

Buffer.isBuffer = function isBuffer (b) {
  return b != null && b._isBuffer === true
};

Buffer.compare = function compare (a, b) {
  if (!Buffer.isBuffer(a) || !Buffer.isBuffer(b)) {
    throw new TypeError('Arguments must be Buffers')
  }

  if (a === b) return 0

  var x = a.length;
  var y = b.length;

  for (var i = 0, len = Math.min(x, y); i < len; ++i) {
    if (a[i] !== b[i]) {
      x = a[i];
      y = b[i];
      break
    }
  }

  if (x < y) return -1
  if (y < x) return 1
  return 0
};

Buffer.isEncoding = function isEncoding (encoding) {
  switch (String(encoding).toLowerCase()) {
    case 'hex':
    case 'utf8':
    case 'utf-8':
    case 'ascii':
    case 'latin1':
    case 'binary':
    case 'base64':
    case 'ucs2':
    case 'ucs-2':
    case 'utf16le':
    case 'utf-16le':
      return true
    default:
      return false
  }
};

Buffer.concat = function concat (list, length) {
  if (!Array.isArray(list)) {
    throw new TypeError('"list" argument must be an Array of Buffers')
  }

  if (list.length === 0) {
    return Buffer.alloc(0)
  }

  var i;
  if (length === undefined) {
    length = 0;
    for (i = 0; i < list.length; ++i) {
      length += list[i].length;
    }
  }

  var buffer = Buffer.allocUnsafe(length);
  var pos = 0;
  for (i = 0; i < list.length; ++i) {
    var buf = list[i];
    if (!Buffer.isBuffer(buf)) {
      throw new TypeError('"list" argument must be an Array of Buffers')
    }
    buf.copy(buffer, pos);
    pos += buf.length;
  }
  return buffer
};

function byteLength (string, encoding) {
  if (Buffer.isBuffer(string)) {
    return string.length
  }
  if (isArrayBufferView(string) || isArrayBuffer(string)) {
    return string.byteLength
  }
  if (typeof string !== 'string') {
    string = '' + string;
  }

  var len = string.length;
  if (len === 0) return 0

  // Use a for loop to avoid recursion
  var loweredCase = false;
  for (;;) {
    switch (encoding) {
      case 'ascii':
      case 'latin1':
      case 'binary':
        return len
      case 'utf8':
      case 'utf-8':
      case undefined:
        return utf8ToBytes(string).length
      case 'ucs2':
      case 'ucs-2':
      case 'utf16le':
      case 'utf-16le':
        return len * 2
      case 'hex':
        return len >>> 1
      case 'base64':
        return base64ToBytes(string).length
      default:
        if (loweredCase) return utf8ToBytes(string).length // assume utf8
        encoding = ('' + encoding).toLowerCase();
        loweredCase = true;
    }
  }
}
Buffer.byteLength = byteLength;

function slowToString (encoding, start, end) {
  var loweredCase = false;

  // No need to verify that "this.length <= MAX_UINT32" since it's a read-only
  // property of a typed array.

  // This behaves neither like String nor Uint8Array in that we set start/end
  // to their upper/lower bounds if the value passed is out of range.
  // undefined is handled specially as per ECMA-262 6th Edition,
  // Section 13.3.3.7 Runtime Semantics: KeyedBindingInitialization.
  if (start === undefined || start < 0) {
    start = 0;
  }
  // Return early if start > this.length. Done here to prevent potential uint32
  // coercion fail below.
  if (start > this.length) {
    return ''
  }

  if (end === undefined || end > this.length) {
    end = this.length;
  }

  if (end <= 0) {
    return ''
  }

  // Force coersion to uint32. This will also coerce falsey/NaN values to 0.
  end >>>= 0;
  start >>>= 0;

  if (end <= start) {
    return ''
  }

  if (!encoding) encoding = 'utf8';

  while (true) {
    switch (encoding) {
      case 'hex':
        return hexSlice(this, start, end)

      case 'utf8':
      case 'utf-8':
        return utf8Slice(this, start, end)

      case 'ascii':
        return asciiSlice(this, start, end)

      case 'latin1':
      case 'binary':
        return latin1Slice(this, start, end)

      case 'base64':
        return base64Slice(this, start, end)

      case 'ucs2':
      case 'ucs-2':
      case 'utf16le':
      case 'utf-16le':
        return utf16leSlice(this, start, end)

      default:
        if (loweredCase) throw new TypeError('Unknown encoding: ' + encoding)
        encoding = (encoding + '').toLowerCase();
        loweredCase = true;
    }
  }
}

// This property is used by `Buffer.isBuffer` (and the `is-buffer` npm package)
// to detect a Buffer instance. It's not possible to use `instanceof Buffer`
// reliably in a browserify context because there could be multiple different
// copies of the 'buffer' package in use. This method works even for Buffer
// instances that were created from another copy of the `buffer` package.
// See: https://github.com/feross/buffer/issues/154
Buffer.prototype._isBuffer = true;

function swap (b, n, m) {
  var i = b[n];
  b[n] = b[m];
  b[m] = i;
}

Buffer.prototype.swap16 = function swap16 () {
  var len = this.length;
  if (len % 2 !== 0) {
    throw new RangeError('Buffer size must be a multiple of 16-bits')
  }
  for (var i = 0; i < len; i += 2) {
    swap(this, i, i + 1);
  }
  return this
};

Buffer.prototype.swap32 = function swap32 () {
  var len = this.length;
  if (len % 4 !== 0) {
    throw new RangeError('Buffer size must be a multiple of 32-bits')
  }
  for (var i = 0; i < len; i += 4) {
    swap(this, i, i + 3);
    swap(this, i + 1, i + 2);
  }
  return this
};

Buffer.prototype.swap64 = function swap64 () {
  var len = this.length;
  if (len % 8 !== 0) {
    throw new RangeError('Buffer size must be a multiple of 64-bits')
  }
  for (var i = 0; i < len; i += 8) {
    swap(this, i, i + 7);
    swap(this, i + 1, i + 6);
    swap(this, i + 2, i + 5);
    swap(this, i + 3, i + 4);
  }
  return this
};

Buffer.prototype.toString = function toString () {
  var length = this.length;
  if (length === 0) return ''
  if (arguments.length === 0) return utf8Slice(this, 0, length)
  return slowToString.apply(this, arguments)
};

Buffer.prototype.equals = function equals (b) {
  if (!Buffer.isBuffer(b)) throw new TypeError('Argument must be a Buffer')
  if (this === b) return true
  return Buffer.compare(this, b) === 0
};

Buffer.prototype.inspect = function inspect () {
  var str = '';
  var max = exports.INSPECT_MAX_BYTES;
  if (this.length > 0) {
    str = this.toString('hex', 0, max).match(/.{2}/g).join(' ');
    if (this.length > max) str += ' ... ';
  }
  return '<Buffer ' + str + '>'
};

Buffer.prototype.compare = function compare (target, start, end, thisStart, thisEnd) {
  if (!Buffer.isBuffer(target)) {
    throw new TypeError('Argument must be a Buffer')
  }

  if (start === undefined) {
    start = 0;
  }
  if (end === undefined) {
    end = target ? target.length : 0;
  }
  if (thisStart === undefined) {
    thisStart = 0;
  }
  if (thisEnd === undefined) {
    thisEnd = this.length;
  }

  if (start < 0 || end > target.length || thisStart < 0 || thisEnd > this.length) {
    throw new RangeError('out of range index')
  }

  if (thisStart >= thisEnd && start >= end) {
    return 0
  }
  if (thisStart >= thisEnd) {
    return -1
  }
  if (start >= end) {
    return 1
  }

  start >>>= 0;
  end >>>= 0;
  thisStart >>>= 0;
  thisEnd >>>= 0;

  if (this === target) return 0

  var x = thisEnd - thisStart;
  var y = end - start;
  var len = Math.min(x, y);

  var thisCopy = this.slice(thisStart, thisEnd);
  var targetCopy = target.slice(start, end);

  for (var i = 0; i < len; ++i) {
    if (thisCopy[i] !== targetCopy[i]) {
      x = thisCopy[i];
      y = targetCopy[i];
      break
    }
  }

  if (x < y) return -1
  if (y < x) return 1
  return 0
};

// Finds either the first index of `val` in `buffer` at offset >= `byteOffset`,
// OR the last index of `val` in `buffer` at offset <= `byteOffset`.
//
// Arguments:
// - buffer - a Buffer to search
// - val - a string, Buffer, or number
// - byteOffset - an index into `buffer`; will be clamped to an int32
// - encoding - an optional encoding, relevant is val is a string
// - dir - true for indexOf, false for lastIndexOf
function bidirectionalIndexOf (buffer, val, byteOffset, encoding, dir) {
  // Empty buffer means no match
  if (buffer.length === 0) return -1

  // Normalize byteOffset
  if (typeof byteOffset === 'string') {
    encoding = byteOffset;
    byteOffset = 0;
  } else if (byteOffset > 0x7fffffff) {
    byteOffset = 0x7fffffff;
  } else if (byteOffset < -0x80000000) {
    byteOffset = -0x80000000;
  }
  byteOffset = +byteOffset;  // Coerce to Number.
  if (numberIsNaN(byteOffset)) {
    // byteOffset: it it's undefined, null, NaN, "foo", etc, search whole buffer
    byteOffset = dir ? 0 : (buffer.length - 1);
  }

  // Normalize byteOffset: negative offsets start from the end of the buffer
  if (byteOffset < 0) byteOffset = buffer.length + byteOffset;
  if (byteOffset >= buffer.length) {
    if (dir) return -1
    else byteOffset = buffer.length - 1;
  } else if (byteOffset < 0) {
    if (dir) byteOffset = 0;
    else return -1
  }

  // Normalize val
  if (typeof val === 'string') {
    val = Buffer.from(val, encoding);
  }

  // Finally, search either indexOf (if dir is true) or lastIndexOf
  if (Buffer.isBuffer(val)) {
    // Special case: looking for empty string/buffer always fails
    if (val.length === 0) {
      return -1
    }
    return arrayIndexOf(buffer, val, byteOffset, encoding, dir)
  } else if (typeof val === 'number') {
    val = val & 0xFF; // Search for a byte value [0-255]
    if (typeof Uint8Array.prototype.indexOf === 'function') {
      if (dir) {
        return Uint8Array.prototype.indexOf.call(buffer, val, byteOffset)
      } else {
        return Uint8Array.prototype.lastIndexOf.call(buffer, val, byteOffset)
      }
    }
    return arrayIndexOf(buffer, [ val ], byteOffset, encoding, dir)
  }

  throw new TypeError('val must be string, number or Buffer')
}

function arrayIndexOf (arr, val, byteOffset, encoding, dir) {
  var indexSize = 1;
  var arrLength = arr.length;
  var valLength = val.length;

  if (encoding !== undefined) {
    encoding = String(encoding).toLowerCase();
    if (encoding === 'ucs2' || encoding === 'ucs-2' ||
        encoding === 'utf16le' || encoding === 'utf-16le') {
      if (arr.length < 2 || val.length < 2) {
        return -1
      }
      indexSize = 2;
      arrLength /= 2;
      valLength /= 2;
      byteOffset /= 2;
    }
  }

  function read (buf, i) {
    if (indexSize === 1) {
      return buf[i]
    } else {
      return buf.readUInt16BE(i * indexSize)
    }
  }

  var i;
  if (dir) {
    var foundIndex = -1;
    for (i = byteOffset; i < arrLength; i++) {
      if (read(arr, i) === read(val, foundIndex === -1 ? 0 : i - foundIndex)) {
        if (foundIndex === -1) foundIndex = i;
        if (i - foundIndex + 1 === valLength) return foundIndex * indexSize
      } else {
        if (foundIndex !== -1) i -= i - foundIndex;
        foundIndex = -1;
      }
    }
  } else {
    if (byteOffset + valLength > arrLength) byteOffset = arrLength - valLength;
    for (i = byteOffset; i >= 0; i--) {
      var found = true;
      for (var j = 0; j < valLength; j++) {
        if (read(arr, i + j) !== read(val, j)) {
          found = false;
          break
        }
      }
      if (found) return i
    }
  }

  return -1
}

Buffer.prototype.includes = function includes (val, byteOffset, encoding) {
  return this.indexOf(val, byteOffset, encoding) !== -1
};

Buffer.prototype.indexOf = function indexOf (val, byteOffset, encoding) {
  return bidirectionalIndexOf(this, val, byteOffset, encoding, true)
};

Buffer.prototype.lastIndexOf = function lastIndexOf (val, byteOffset, encoding) {
  return bidirectionalIndexOf(this, val, byteOffset, encoding, false)
};

function hexWrite (buf, string, offset, length) {
  offset = Number(offset) || 0;
  var remaining = buf.length - offset;
  if (!length) {
    length = remaining;
  } else {
    length = Number(length);
    if (length > remaining) {
      length = remaining;
    }
  }

  // must be an even number of digits
  var strLen = string.length;
  if (strLen % 2 !== 0) throw new TypeError('Invalid hex string')

  if (length > strLen / 2) {
    length = strLen / 2;
  }
  for (var i = 0; i < length; ++i) {
    var parsed = parseInt(string.substr(i * 2, 2), 16);
    if (numberIsNaN(parsed)) return i
    buf[offset + i] = parsed;
  }
  return i
}

function utf8Write (buf, string, offset, length) {
  return blitBuffer(utf8ToBytes(string, buf.length - offset), buf, offset, length)
}

function asciiWrite (buf, string, offset, length) {
  return blitBuffer(asciiToBytes(string), buf, offset, length)
}

function latin1Write (buf, string, offset, length) {
  return asciiWrite(buf, string, offset, length)
}

function base64Write (buf, string, offset, length) {
  return blitBuffer(base64ToBytes(string), buf, offset, length)
}

function ucs2Write (buf, string, offset, length) {
  return blitBuffer(utf16leToBytes(string, buf.length - offset), buf, offset, length)
}

Buffer.prototype.write = function write (string, offset, length, encoding) {
  // Buffer#write(string)
  if (offset === undefined) {
    encoding = 'utf8';
    length = this.length;
    offset = 0;
  // Buffer#write(string, encoding)
  } else if (length === undefined && typeof offset === 'string') {
    encoding = offset;
    length = this.length;
    offset = 0;
  // Buffer#write(string, offset[, length][, encoding])
  } else if (isFinite(offset)) {
    offset = offset >>> 0;
    if (isFinite(length)) {
      length = length >>> 0;
      if (encoding === undefined) encoding = 'utf8';
    } else {
      encoding = length;
      length = undefined;
    }
  } else {
    throw new Error(
      'Buffer.write(string, encoding, offset[, length]) is no longer supported'
    )
  }

  var remaining = this.length - offset;
  if (length === undefined || length > remaining) length = remaining;

  if ((string.length > 0 && (length < 0 || offset < 0)) || offset > this.length) {
    throw new RangeError('Attempt to write outside buffer bounds')
  }

  if (!encoding) encoding = 'utf8';

  var loweredCase = false;
  for (;;) {
    switch (encoding) {
      case 'hex':
        return hexWrite(this, string, offset, length)

      case 'utf8':
      case 'utf-8':
        return utf8Write(this, string, offset, length)

      case 'ascii':
        return asciiWrite(this, string, offset, length)

      case 'latin1':
      case 'binary':
        return latin1Write(this, string, offset, length)

      case 'base64':
        // Warning: maxLength not taken into account in base64Write
        return base64Write(this, string, offset, length)

      case 'ucs2':
      case 'ucs-2':
      case 'utf16le':
      case 'utf-16le':
        return ucs2Write(this, string, offset, length)

      default:
        if (loweredCase) throw new TypeError('Unknown encoding: ' + encoding)
        encoding = ('' + encoding).toLowerCase();
        loweredCase = true;
    }
  }
};

Buffer.prototype.toJSON = function toJSON () {
  return {
    type: 'Buffer',
    data: Array.prototype.slice.call(this._arr || this, 0)
  }
};

function base64Slice (buf, start, end) {
  if (start === 0 && end === buf.length) {
    return base64Js.fromByteArray(buf)
  } else {
    return base64Js.fromByteArray(buf.slice(start, end))
  }
}

function utf8Slice (buf, start, end) {
  end = Math.min(buf.length, end);
  var res = [];

  var i = start;
  while (i < end) {
    var firstByte = buf[i];
    var codePoint = null;
    var bytesPerSequence = (firstByte > 0xEF) ? 4
      : (firstByte > 0xDF) ? 3
      : (firstByte > 0xBF) ? 2
      : 1;

    if (i + bytesPerSequence <= end) {
      var secondByte, thirdByte, fourthByte, tempCodePoint;

      switch (bytesPerSequence) {
        case 1:
          if (firstByte < 0x80) {
            codePoint = firstByte;
          }
          break
        case 2:
          secondByte = buf[i + 1];
          if ((secondByte & 0xC0) === 0x80) {
            tempCodePoint = (firstByte & 0x1F) << 0x6 | (secondByte & 0x3F);
            if (tempCodePoint > 0x7F) {
              codePoint = tempCodePoint;
            }
          }
          break
        case 3:
          secondByte = buf[i + 1];
          thirdByte = buf[i + 2];
          if ((secondByte & 0xC0) === 0x80 && (thirdByte & 0xC0) === 0x80) {
            tempCodePoint = (firstByte & 0xF) << 0xC | (secondByte & 0x3F) << 0x6 | (thirdByte & 0x3F);
            if (tempCodePoint > 0x7FF && (tempCodePoint < 0xD800 || tempCodePoint > 0xDFFF)) {
              codePoint = tempCodePoint;
            }
          }
          break
        case 4:
          secondByte = buf[i + 1];
          thirdByte = buf[i + 2];
          fourthByte = buf[i + 3];
          if ((secondByte & 0xC0) === 0x80 && (thirdByte & 0xC0) === 0x80 && (fourthByte & 0xC0) === 0x80) {
            tempCodePoint = (firstByte & 0xF) << 0x12 | (secondByte & 0x3F) << 0xC | (thirdByte & 0x3F) << 0x6 | (fourthByte & 0x3F);
            if (tempCodePoint > 0xFFFF && tempCodePoint < 0x110000) {
              codePoint = tempCodePoint;
            }
          }
      }
    }

    if (codePoint === null) {
      // we did not generate a valid codePoint so insert a
      // replacement char (U+FFFD) and advance only 1 byte
      codePoint = 0xFFFD;
      bytesPerSequence = 1;
    } else if (codePoint > 0xFFFF) {
      // encode to utf16 (surrogate pair dance)
      codePoint -= 0x10000;
      res.push(codePoint >>> 10 & 0x3FF | 0xD800);
      codePoint = 0xDC00 | codePoint & 0x3FF;
    }

    res.push(codePoint);
    i += bytesPerSequence;
  }

  return decodeCodePointsArray(res)
}

// Based on http://stackoverflow.com/a/22747272/680742, the browser with
// the lowest limit is Chrome, with 0x10000 args.
// We go 1 magnitude less, for safety
var MAX_ARGUMENTS_LENGTH = 0x1000;

function decodeCodePointsArray (codePoints) {
  var len = codePoints.length;
  if (len <= MAX_ARGUMENTS_LENGTH) {
    return String.fromCharCode.apply(String, codePoints) // avoid extra slice()
  }

  // Decode in chunks to avoid "call stack size exceeded".
  var res = '';
  var i = 0;
  while (i < len) {
    res += String.fromCharCode.apply(
      String,
      codePoints.slice(i, i += MAX_ARGUMENTS_LENGTH)
    );
  }
  return res
}

function asciiSlice (buf, start, end) {
  var ret = '';
  end = Math.min(buf.length, end);

  for (var i = start; i < end; ++i) {
    ret += String.fromCharCode(buf[i] & 0x7F);
  }
  return ret
}

function latin1Slice (buf, start, end) {
  var ret = '';
  end = Math.min(buf.length, end);

  for (var i = start; i < end; ++i) {
    ret += String.fromCharCode(buf[i]);
  }
  return ret
}

function hexSlice (buf, start, end) {
  var len = buf.length;

  if (!start || start < 0) start = 0;
  if (!end || end < 0 || end > len) end = len;

  var out = '';
  for (var i = start; i < end; ++i) {
    out += toHex(buf[i]);
  }
  return out
}

function utf16leSlice (buf, start, end) {
  var bytes = buf.slice(start, end);
  var res = '';
  for (var i = 0; i < bytes.length; i += 2) {
    res += String.fromCharCode(bytes[i] + (bytes[i + 1] * 256));
  }
  return res
}

Buffer.prototype.slice = function slice (start, end) {
  var len = this.length;
  start = ~~start;
  end = end === undefined ? len : ~~end;

  if (start < 0) {
    start += len;
    if (start < 0) start = 0;
  } else if (start > len) {
    start = len;
  }

  if (end < 0) {
    end += len;
    if (end < 0) end = 0;
  } else if (end > len) {
    end = len;
  }

  if (end < start) end = start;

  var newBuf = this.subarray(start, end);
  // Return an augmented `Uint8Array` instance
  newBuf.__proto__ = Buffer.prototype;
  return newBuf
};

/*
 * Need to make sure that buffer isn't trying to write out of bounds.
 */
function checkOffset (offset, ext, length) {
  if ((offset % 1) !== 0 || offset < 0) throw new RangeError('offset is not uint')
  if (offset + ext > length) throw new RangeError('Trying to access beyond buffer length')
}

Buffer.prototype.readUIntLE = function readUIntLE (offset, byteLength, noAssert) {
  offset = offset >>> 0;
  byteLength = byteLength >>> 0;
  if (!noAssert) checkOffset(offset, byteLength, this.length);

  var val = this[offset];
  var mul = 1;
  var i = 0;
  while (++i < byteLength && (mul *= 0x100)) {
    val += this[offset + i] * mul;
  }

  return val
};

Buffer.prototype.readUIntBE = function readUIntBE (offset, byteLength, noAssert) {
  offset = offset >>> 0;
  byteLength = byteLength >>> 0;
  if (!noAssert) {
    checkOffset(offset, byteLength, this.length);
  }

  var val = this[offset + --byteLength];
  var mul = 1;
  while (byteLength > 0 && (mul *= 0x100)) {
    val += this[offset + --byteLength] * mul;
  }

  return val
};

Buffer.prototype.readUInt8 = function readUInt8 (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 1, this.length);
  return this[offset]
};

Buffer.prototype.readUInt16LE = function readUInt16LE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 2, this.length);
  return this[offset] | (this[offset + 1] << 8)
};

Buffer.prototype.readUInt16BE = function readUInt16BE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 2, this.length);
  return (this[offset] << 8) | this[offset + 1]
};

Buffer.prototype.readUInt32LE = function readUInt32LE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 4, this.length);

  return ((this[offset]) |
      (this[offset + 1] << 8) |
      (this[offset + 2] << 16)) +
      (this[offset + 3] * 0x1000000)
};

Buffer.prototype.readUInt32BE = function readUInt32BE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 4, this.length);

  return (this[offset] * 0x1000000) +
    ((this[offset + 1] << 16) |
    (this[offset + 2] << 8) |
    this[offset + 3])
};

Buffer.prototype.readIntLE = function readIntLE (offset, byteLength, noAssert) {
  offset = offset >>> 0;
  byteLength = byteLength >>> 0;
  if (!noAssert) checkOffset(offset, byteLength, this.length);

  var val = this[offset];
  var mul = 1;
  var i = 0;
  while (++i < byteLength && (mul *= 0x100)) {
    val += this[offset + i] * mul;
  }
  mul *= 0x80;

  if (val >= mul) val -= Math.pow(2, 8 * byteLength);

  return val
};

Buffer.prototype.readIntBE = function readIntBE (offset, byteLength, noAssert) {
  offset = offset >>> 0;
  byteLength = byteLength >>> 0;
  if (!noAssert) checkOffset(offset, byteLength, this.length);

  var i = byteLength;
  var mul = 1;
  var val = this[offset + --i];
  while (i > 0 && (mul *= 0x100)) {
    val += this[offset + --i] * mul;
  }
  mul *= 0x80;

  if (val >= mul) val -= Math.pow(2, 8 * byteLength);

  return val
};

Buffer.prototype.readInt8 = function readInt8 (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 1, this.length);
  if (!(this[offset] & 0x80)) return (this[offset])
  return ((0xff - this[offset] + 1) * -1)
};

Buffer.prototype.readInt16LE = function readInt16LE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 2, this.length);
  var val = this[offset] | (this[offset + 1] << 8);
  return (val & 0x8000) ? val | 0xFFFF0000 : val
};

Buffer.prototype.readInt16BE = function readInt16BE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 2, this.length);
  var val = this[offset + 1] | (this[offset] << 8);
  return (val & 0x8000) ? val | 0xFFFF0000 : val
};

Buffer.prototype.readInt32LE = function readInt32LE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 4, this.length);

  return (this[offset]) |
    (this[offset + 1] << 8) |
    (this[offset + 2] << 16) |
    (this[offset + 3] << 24)
};

Buffer.prototype.readInt32BE = function readInt32BE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 4, this.length);

  return (this[offset] << 24) |
    (this[offset + 1] << 16) |
    (this[offset + 2] << 8) |
    (this[offset + 3])
};

Buffer.prototype.readFloatLE = function readFloatLE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 4, this.length);
  return ieee754.read(this, offset, true, 23, 4)
};

Buffer.prototype.readFloatBE = function readFloatBE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 4, this.length);
  return ieee754.read(this, offset, false, 23, 4)
};

Buffer.prototype.readDoubleLE = function readDoubleLE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 8, this.length);
  return ieee754.read(this, offset, true, 52, 8)
};

Buffer.prototype.readDoubleBE = function readDoubleBE (offset, noAssert) {
  offset = offset >>> 0;
  if (!noAssert) checkOffset(offset, 8, this.length);
  return ieee754.read(this, offset, false, 52, 8)
};

function checkInt (buf, value, offset, ext, max, min) {
  if (!Buffer.isBuffer(buf)) throw new TypeError('"buffer" argument must be a Buffer instance')
  if (value > max || value < min) throw new RangeError('"value" argument is out of bounds')
  if (offset + ext > buf.length) throw new RangeError('Index out of range')
}

Buffer.prototype.writeUIntLE = function writeUIntLE (value, offset, byteLength, noAssert) {
  value = +value;
  offset = offset >>> 0;
  byteLength = byteLength >>> 0;
  if (!noAssert) {
    var maxBytes = Math.pow(2, 8 * byteLength) - 1;
    checkInt(this, value, offset, byteLength, maxBytes, 0);
  }

  var mul = 1;
  var i = 0;
  this[offset] = value & 0xFF;
  while (++i < byteLength && (mul *= 0x100)) {
    this[offset + i] = (value / mul) & 0xFF;
  }

  return offset + byteLength
};

Buffer.prototype.writeUIntBE = function writeUIntBE (value, offset, byteLength, noAssert) {
  value = +value;
  offset = offset >>> 0;
  byteLength = byteLength >>> 0;
  if (!noAssert) {
    var maxBytes = Math.pow(2, 8 * byteLength) - 1;
    checkInt(this, value, offset, byteLength, maxBytes, 0);
  }

  var i = byteLength - 1;
  var mul = 1;
  this[offset + i] = value & 0xFF;
  while (--i >= 0 && (mul *= 0x100)) {
    this[offset + i] = (value / mul) & 0xFF;
  }

  return offset + byteLength
};

Buffer.prototype.writeUInt8 = function writeUInt8 (value, offset, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) checkInt(this, value, offset, 1, 0xff, 0);
  this[offset] = (value & 0xff);
  return offset + 1
};

Buffer.prototype.writeUInt16LE = function writeUInt16LE (value, offset, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) checkInt(this, value, offset, 2, 0xffff, 0);
  this[offset] = (value & 0xff);
  this[offset + 1] = (value >>> 8);
  return offset + 2
};

Buffer.prototype.writeUInt16BE = function writeUInt16BE (value, offset, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) checkInt(this, value, offset, 2, 0xffff, 0);
  this[offset] = (value >>> 8);
  this[offset + 1] = (value & 0xff);
  return offset + 2
};

Buffer.prototype.writeUInt32LE = function writeUInt32LE (value, offset, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) checkInt(this, value, offset, 4, 0xffffffff, 0);
  this[offset + 3] = (value >>> 24);
  this[offset + 2] = (value >>> 16);
  this[offset + 1] = (value >>> 8);
  this[offset] = (value & 0xff);
  return offset + 4
};

Buffer.prototype.writeUInt32BE = function writeUInt32BE (value, offset, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) checkInt(this, value, offset, 4, 0xffffffff, 0);
  this[offset] = (value >>> 24);
  this[offset + 1] = (value >>> 16);
  this[offset + 2] = (value >>> 8);
  this[offset + 3] = (value & 0xff);
  return offset + 4
};

Buffer.prototype.writeIntLE = function writeIntLE (value, offset, byteLength, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) {
    var limit = Math.pow(2, (8 * byteLength) - 1);

    checkInt(this, value, offset, byteLength, limit - 1, -limit);
  }

  var i = 0;
  var mul = 1;
  var sub = 0;
  this[offset] = value & 0xFF;
  while (++i < byteLength && (mul *= 0x100)) {
    if (value < 0 && sub === 0 && this[offset + i - 1] !== 0) {
      sub = 1;
    }
    this[offset + i] = ((value / mul) >> 0) - sub & 0xFF;
  }

  return offset + byteLength
};

Buffer.prototype.writeIntBE = function writeIntBE (value, offset, byteLength, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) {
    var limit = Math.pow(2, (8 * byteLength) - 1);

    checkInt(this, value, offset, byteLength, limit - 1, -limit);
  }

  var i = byteLength - 1;
  var mul = 1;
  var sub = 0;
  this[offset + i] = value & 0xFF;
  while (--i >= 0 && (mul *= 0x100)) {
    if (value < 0 && sub === 0 && this[offset + i + 1] !== 0) {
      sub = 1;
    }
    this[offset + i] = ((value / mul) >> 0) - sub & 0xFF;
  }

  return offset + byteLength
};

Buffer.prototype.writeInt8 = function writeInt8 (value, offset, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) checkInt(this, value, offset, 1, 0x7f, -0x80);
  if (value < 0) value = 0xff + value + 1;
  this[offset] = (value & 0xff);
  return offset + 1
};

Buffer.prototype.writeInt16LE = function writeInt16LE (value, offset, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) checkInt(this, value, offset, 2, 0x7fff, -0x8000);
  this[offset] = (value & 0xff);
  this[offset + 1] = (value >>> 8);
  return offset + 2
};

Buffer.prototype.writeInt16BE = function writeInt16BE (value, offset, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) checkInt(this, value, offset, 2, 0x7fff, -0x8000);
  this[offset] = (value >>> 8);
  this[offset + 1] = (value & 0xff);
  return offset + 2
};

Buffer.prototype.writeInt32LE = function writeInt32LE (value, offset, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) checkInt(this, value, offset, 4, 0x7fffffff, -0x80000000);
  this[offset] = (value & 0xff);
  this[offset + 1] = (value >>> 8);
  this[offset + 2] = (value >>> 16);
  this[offset + 3] = (value >>> 24);
  return offset + 4
};

Buffer.prototype.writeInt32BE = function writeInt32BE (value, offset, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) checkInt(this, value, offset, 4, 0x7fffffff, -0x80000000);
  if (value < 0) value = 0xffffffff + value + 1;
  this[offset] = (value >>> 24);
  this[offset + 1] = (value >>> 16);
  this[offset + 2] = (value >>> 8);
  this[offset + 3] = (value & 0xff);
  return offset + 4
};

function checkIEEE754 (buf, value, offset, ext, max, min) {
  if (offset + ext > buf.length) throw new RangeError('Index out of range')
  if (offset < 0) throw new RangeError('Index out of range')
}

function writeFloat (buf, value, offset, littleEndian, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) {
    checkIEEE754(buf, value, offset, 4, 3.4028234663852886e+38, -3.4028234663852886e+38);
  }
  ieee754.write(buf, value, offset, littleEndian, 23, 4);
  return offset + 4
}

Buffer.prototype.writeFloatLE = function writeFloatLE (value, offset, noAssert) {
  return writeFloat(this, value, offset, true, noAssert)
};

Buffer.prototype.writeFloatBE = function writeFloatBE (value, offset, noAssert) {
  return writeFloat(this, value, offset, false, noAssert)
};

function writeDouble (buf, value, offset, littleEndian, noAssert) {
  value = +value;
  offset = offset >>> 0;
  if (!noAssert) {
    checkIEEE754(buf, value, offset, 8, 1.7976931348623157E+308, -1.7976931348623157E+308);
  }
  ieee754.write(buf, value, offset, littleEndian, 52, 8);
  return offset + 8
}

Buffer.prototype.writeDoubleLE = function writeDoubleLE (value, offset, noAssert) {
  return writeDouble(this, value, offset, true, noAssert)
};

Buffer.prototype.writeDoubleBE = function writeDoubleBE (value, offset, noAssert) {
  return writeDouble(this, value, offset, false, noAssert)
};

// copy(targetBuffer, targetStart=0, sourceStart=0, sourceEnd=buffer.length)
Buffer.prototype.copy = function copy (target, targetStart, start, end) {
  if (!start) start = 0;
  if (!end && end !== 0) end = this.length;
  if (targetStart >= target.length) targetStart = target.length;
  if (!targetStart) targetStart = 0;
  if (end > 0 && end < start) end = start;

  // Copy 0 bytes; we're done
  if (end === start) return 0
  if (target.length === 0 || this.length === 0) return 0

  // Fatal error conditions
  if (targetStart < 0) {
    throw new RangeError('targetStart out of bounds')
  }
  if (start < 0 || start >= this.length) throw new RangeError('sourceStart out of bounds')
  if (end < 0) throw new RangeError('sourceEnd out of bounds')

  // Are we oob?
  if (end > this.length) end = this.length;
  if (target.length - targetStart < end - start) {
    end = target.length - targetStart + start;
  }

  var len = end - start;
  var i;

  if (this === target && start < targetStart && targetStart < end) {
    // descending copy from end
    for (i = len - 1; i >= 0; --i) {
      target[i + targetStart] = this[i + start];
    }
  } else if (len < 1000) {
    // ascending copy from start
    for (i = 0; i < len; ++i) {
      target[i + targetStart] = this[i + start];
    }
  } else {
    Uint8Array.prototype.set.call(
      target,
      this.subarray(start, start + len),
      targetStart
    );
  }

  return len
};

// Usage:
//    buffer.fill(number[, offset[, end]])
//    buffer.fill(buffer[, offset[, end]])
//    buffer.fill(string[, offset[, end]][, encoding])
Buffer.prototype.fill = function fill (val, start, end, encoding) {
  // Handle string cases:
  if (typeof val === 'string') {
    if (typeof start === 'string') {
      encoding = start;
      start = 0;
      end = this.length;
    } else if (typeof end === 'string') {
      encoding = end;
      end = this.length;
    }
    if (val.length === 1) {
      var code = val.charCodeAt(0);
      if (code < 256) {
        val = code;
      }
    }
    if (encoding !== undefined && typeof encoding !== 'string') {
      throw new TypeError('encoding must be a string')
    }
    if (typeof encoding === 'string' && !Buffer.isEncoding(encoding)) {
      throw new TypeError('Unknown encoding: ' + encoding)
    }
  } else if (typeof val === 'number') {
    val = val & 255;
  }

  // Invalid ranges are not set to a default, so can range check early.
  if (start < 0 || this.length < start || this.length < end) {
    throw new RangeError('Out of range index')
  }

  if (end <= start) {
    return this
  }

  start = start >>> 0;
  end = end === undefined ? this.length : end >>> 0;

  if (!val) val = 0;

  var i;
  if (typeof val === 'number') {
    for (i = start; i < end; ++i) {
      this[i] = val;
    }
  } else {
    var bytes = Buffer.isBuffer(val)
      ? val
      : new Buffer(val, encoding);
    var len = bytes.length;
    for (i = 0; i < end - start; ++i) {
      this[i + start] = bytes[i % len];
    }
  }

  return this
};

// HELPER FUNCTIONS
// ================

var INVALID_BASE64_RE = /[^+/0-9A-Za-z-_]/g;

function base64clean (str) {
  // Node strips out invalid characters like \n and \t from the string, base64-js does not
  str = str.trim().replace(INVALID_BASE64_RE, '');
  // Node converts strings with length < 2 to ''
  if (str.length < 2) return ''
  // Node allows for non-padded base64 strings (missing trailing ===), base64-js does not
  while (str.length % 4 !== 0) {
    str = str + '=';
  }
  return str
}

function toHex (n) {
  if (n < 16) return '0' + n.toString(16)
  return n.toString(16)
}

function utf8ToBytes (string, units) {
  units = units || Infinity;
  var codePoint;
  var length = string.length;
  var leadSurrogate = null;
  var bytes = [];

  for (var i = 0; i < length; ++i) {
    codePoint = string.charCodeAt(i);

    // is surrogate component
    if (codePoint > 0xD7FF && codePoint < 0xE000) {
      // last char was a lead
      if (!leadSurrogate) {
        // no lead yet
        if (codePoint > 0xDBFF) {
          // unexpected trail
          if ((units -= 3) > -1) bytes.push(0xEF, 0xBF, 0xBD);
          continue
        } else if (i + 1 === length) {
          // unpaired lead
          if ((units -= 3) > -1) bytes.push(0xEF, 0xBF, 0xBD);
          continue
        }

        // valid lead
        leadSurrogate = codePoint;

        continue
      }

      // 2 leads in a row
      if (codePoint < 0xDC00) {
        if ((units -= 3) > -1) bytes.push(0xEF, 0xBF, 0xBD);
        leadSurrogate = codePoint;
        continue
      }

      // valid surrogate pair
      codePoint = (leadSurrogate - 0xD800 << 10 | codePoint - 0xDC00) + 0x10000;
    } else if (leadSurrogate) {
      // valid bmp char, but last char was a lead
      if ((units -= 3) > -1) bytes.push(0xEF, 0xBF, 0xBD);
    }

    leadSurrogate = null;

    // encode utf8
    if (codePoint < 0x80) {
      if ((units -= 1) < 0) break
      bytes.push(codePoint);
    } else if (codePoint < 0x800) {
      if ((units -= 2) < 0) break
      bytes.push(
        codePoint >> 0x6 | 0xC0,
        codePoint & 0x3F | 0x80
      );
    } else if (codePoint < 0x10000) {
      if ((units -= 3) < 0) break
      bytes.push(
        codePoint >> 0xC | 0xE0,
        codePoint >> 0x6 & 0x3F | 0x80,
        codePoint & 0x3F | 0x80
      );
    } else if (codePoint < 0x110000) {
      if ((units -= 4) < 0) break
      bytes.push(
        codePoint >> 0x12 | 0xF0,
        codePoint >> 0xC & 0x3F | 0x80,
        codePoint >> 0x6 & 0x3F | 0x80,
        codePoint & 0x3F | 0x80
      );
    } else {
      throw new Error('Invalid code point')
    }
  }

  return bytes
}

function asciiToBytes (str) {
  var byteArray = [];
  for (var i = 0; i < str.length; ++i) {
    // Node's code seems to be doing this and not & 0x7F..
    byteArray.push(str.charCodeAt(i) & 0xFF);
  }
  return byteArray
}

function utf16leToBytes (str, units) {
  var c, hi, lo;
  var byteArray = [];
  for (var i = 0; i < str.length; ++i) {
    if ((units -= 2) < 0) break

    c = str.charCodeAt(i);
    hi = c >> 8;
    lo = c % 256;
    byteArray.push(lo);
    byteArray.push(hi);
  }

  return byteArray
}

function base64ToBytes (str) {
  return base64Js.toByteArray(base64clean(str))
}

function blitBuffer (src, dst, offset, length) {
  for (var i = 0; i < length; ++i) {
    if ((i + offset >= dst.length) || (i >= src.length)) break
    dst[i + offset] = src[i];
  }
  return i
}

// ArrayBuffers from another context (i.e. an iframe) do not pass the `instanceof` check
// but they should be treated as valid. See: https://github.com/feross/buffer/issues/166
function isArrayBuffer (obj) {
  return obj instanceof ArrayBuffer ||
    (obj != null && obj.constructor != null && obj.constructor.name === 'ArrayBuffer' &&
      typeof obj.byteLength === 'number')
}

// Node 0.10 supports `ArrayBuffer` but lacks `ArrayBuffer.isView`
function isArrayBufferView (obj) {
  return (typeof ArrayBuffer.isView === 'function') && ArrayBuffer.isView(obj)
}

function numberIsNaN (obj) {
  return obj !== obj // eslint-disable-line no-self-compare
}
});

var buffer_1 = buffer.Buffer;
var buffer_2 = buffer.SlowBuffer;
var buffer_3 = buffer.INSPECT_MAX_BYTES;
var buffer_4 = buffer.kMaxLength;

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
/** Error thrown when an HTTP request fails. */
var HttpError = /** @class */ (function (_super) {
    tslib_1.__extends(HttpError, _super);
    /** Constructs a new instance of {@link HttpError}.
     *
     * @param {string} errorMessage A descriptive error message.
     * @param {number} statusCode The HTTP status code represented by this error.
     */
    function HttpError(errorMessage, statusCode) {
        var _newTarget = this.constructor;
        var _this = this;
        var trueProto = _newTarget.prototype;
        _this = _super.call(this, errorMessage) || this;
        _this.statusCode = statusCode;
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        _this.__proto__ = trueProto;
        return _this;
    }
    return HttpError;
}(Error));
/** Error thrown when a timeout elapses. */
var TimeoutError = /** @class */ (function (_super) {
    tslib_1.__extends(TimeoutError, _super);
    /** Constructs a new instance of {@link TimeoutError}.
     *
     * @param {string} errorMessage A descriptive error message.
     */
    function TimeoutError(errorMessage) {
        var _newTarget = this.constructor;
        if (errorMessage === void 0) { errorMessage = "A timeout occurred."; }
        var _this = this;
        var trueProto = _newTarget.prototype;
        _this = _super.call(this, errorMessage) || this;
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        _this.__proto__ = trueProto;
        return _this;
    }
    return TimeoutError;
}(Error));
/** Error thrown when an action is aborted. */
var AbortError = /** @class */ (function (_super) {
    tslib_1.__extends(AbortError, _super);
    /** Constructs a new instance of {@link AbortError}.
     *
     * @param {string} errorMessage A descriptive error message.
     */
    function AbortError(errorMessage) {
        var _newTarget = this.constructor;
        if (errorMessage === void 0) { errorMessage = "An abort occurred."; }
        var _this = this;
        var trueProto = _newTarget.prototype;
        _this = _super.call(this, errorMessage) || this;
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        _this.__proto__ = trueProto;
        return _this;
    }
    return AbortError;
}(Error));

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// These values are designed to match the ASP.NET Log Levels since that's the pattern we're emulating here.
/** Indicates the severity of a log message.
 *
 * Log Levels are ordered in increasing severity. So `Debug` is more severe than `Trace`, etc.
 */
var LogLevel;
(function (LogLevel) {
    /** Log level for very low severity diagnostic messages. */
    LogLevel[LogLevel["Trace"] = 0] = "Trace";
    /** Log level for low severity diagnostic messages. */
    LogLevel[LogLevel["Debug"] = 1] = "Debug";
    /** Log level for informational diagnostic messages. */
    LogLevel[LogLevel["Information"] = 2] = "Information";
    /** Log level for diagnostic messages that indicate a non-fatal problem. */
    LogLevel[LogLevel["Warning"] = 3] = "Warning";
    /** Log level for diagnostic messages that indicate a failure in the current operation. */
    LogLevel[LogLevel["Error"] = 4] = "Error";
    /** Log level for diagnostic messages that indicate a failure that will terminate the entire application. */
    LogLevel[LogLevel["Critical"] = 5] = "Critical";
    /** The highest possible log level. Used when configuring logging to indicate that no log messages should be emitted. */
    LogLevel[LogLevel["None"] = 6] = "None";
})(LogLevel || (LogLevel = {}));

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
/** Represents an HTTP response. */
var HttpResponse = /** @class */ (function () {
    function HttpResponse(statusCode, statusText, content) {
        this.statusCode = statusCode;
        this.statusText = statusText;
        this.content = content;
    }
    return HttpResponse;
}());
/** Abstraction over an HTTP client.
 *
 * This class provides an abstraction over an HTTP client so that a different implementation can be provided on different platforms.
 */
var HttpClient = /** @class */ (function () {
    function HttpClient() {
    }
    HttpClient.prototype.get = function (url, options) {
        return this.send(tslib_1.__assign({}, options, { method: "GET", url: url }));
    };
    HttpClient.prototype.post = function (url, options) {
        return this.send(tslib_1.__assign({}, options, { method: "POST", url: url }));
    };
    HttpClient.prototype.delete = function (url, options) {
        return this.send(tslib_1.__assign({}, options, { method: "DELETE", url: url }));
    };
    return HttpClient;
}());
/** Default implementation of {@link HttpClient}. */
var DefaultHttpClient = /** @class */ (function (_super) {
    tslib_1.__extends(DefaultHttpClient, _super);
    /** Creates a new instance of the {@link DefaultHttpClient}, using the provided {@link ILogger} to log messages. */
    function DefaultHttpClient(logger) {
        var _this = _super.call(this) || this;
        _this.logger = logger;
        return _this;
    }
    /** @inheritDoc */
    DefaultHttpClient.prototype.send = function (request) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            // Check that abort was not signaled before calling send
            if (request.abortSignal && request.abortSignal.aborted) {
                reject(new AbortError());
                return;
            }
            var xhr = new XMLHttpRequest();
            if (!request.method) {
                reject(new Error("No method defined."));
                return;
            }
            if (!request.url) {
                reject(new Error("No url defined."));
                return;
            }
            xhr.open(request.method, request.url, true);
            xhr.withCredentials = true;
            xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
            // Explicitly setting the Content-Type header for React Native on Android platform.
            xhr.setRequestHeader("Content-Type", "text/plain;charset=UTF-8");
            var headers = request.headers;
            if (headers) {
                Object.keys(headers)
                    .forEach(function (header) {
                    xhr.setRequestHeader(header, headers[header]);
                });
            }
            if (request.responseType) {
                xhr.responseType = request.responseType;
            }
            if (request.abortSignal) {
                request.abortSignal.onabort = function () {
                    xhr.abort();
                    reject(new AbortError());
                };
            }
            if (request.timeout) {
                xhr.timeout = request.timeout;
            }
            xhr.onload = function () {
                if (request.abortSignal) {
                    request.abortSignal.onabort = null;
                }
                if (xhr.status >= 200 && xhr.status < 300) {
                    resolve(new HttpResponse(xhr.status, xhr.statusText, xhr.response || xhr.responseText));
                }
                else {
                    reject(new HttpError(xhr.statusText, xhr.status));
                }
            };
            xhr.onerror = function () {
                _this.logger.log(LogLevel.Warning, "Error from HTTP request. " + xhr.status + ": " + xhr.statusText);
                reject(new HttpError(xhr.statusText, xhr.status));
            };
            xhr.ontimeout = function () {
                _this.logger.log(LogLevel.Warning, "Timeout from HTTP request.");
                reject(new TimeoutError());
            };
            xhr.send(request.content || "");
        });
    };
    return DefaultHttpClient;
}(HttpClient));

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Not exported from index
var TextMessageFormat = /** @class */ (function () {
    function TextMessageFormat() {
    }
    TextMessageFormat.write = function (output) {
        return "" + output + TextMessageFormat.RecordSeparator;
    };
    TextMessageFormat.parse = function (input) {
        if (input[input.length - 1] !== TextMessageFormat.RecordSeparator) {
            throw new Error("Message is incomplete.");
        }
        var messages = input.split(TextMessageFormat.RecordSeparator);
        messages.pop();
        return messages;
    };
    TextMessageFormat.RecordSeparatorCode = 0x1e;
    TextMessageFormat.RecordSeparator = String.fromCharCode(TextMessageFormat.RecordSeparatorCode);
    return TextMessageFormat;
}());

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var HandshakeProtocol = /** @class */ (function () {
    function HandshakeProtocol() {
    }
    // Handshake request is always JSON
    HandshakeProtocol.prototype.writeHandshakeRequest = function (handshakeRequest) {
        return TextMessageFormat.write(JSON.stringify(handshakeRequest));
    };
    HandshakeProtocol.prototype.parseHandshakeResponse = function (data) {
        var responseMessage;
        var messageData;
        var remainingData;
        if (data instanceof ArrayBuffer) {
            // Format is binary but still need to read JSON text from handshake response
            var binaryData = new Uint8Array(data);
            var separatorIndex = binaryData.indexOf(TextMessageFormat.RecordSeparatorCode);
            if (separatorIndex === -1) {
                throw new Error("Message is incomplete.");
            }
            // content before separator is handshake response
            // optional content after is additional messages
            var responseLength = separatorIndex + 1;
            messageData = String.fromCharCode.apply(null, binaryData.slice(0, responseLength));
            remainingData = (binaryData.byteLength > responseLength) ? binaryData.slice(responseLength).buffer : null;
        }
        else {
            var textData = data;
            var separatorIndex = textData.indexOf(TextMessageFormat.RecordSeparator);
            if (separatorIndex === -1) {
                throw new Error("Message is incomplete.");
            }
            // content before separator is handshake response
            // optional content after is additional messages
            var responseLength = separatorIndex + 1;
            messageData = textData.substring(0, responseLength);
            remainingData = (textData.length > responseLength) ? textData.substring(responseLength) : null;
        }
        // At this point we should have just the single handshake message
        var messages = TextMessageFormat.parse(messageData);
        responseMessage = JSON.parse(messages[0]);
        // multiple messages could have arrived with handshake
        // return additional data to be parsed as usual, or null if all parsed
        return [remainingData, responseMessage];
    };
    return HandshakeProtocol;
}());

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
/** Defines the type of a Hub Message. */
var MessageType;
(function (MessageType) {
    /** Indicates the message is an Invocation message and implements the {@link InvocationMessage} interface. */
    MessageType[MessageType["Invocation"] = 1] = "Invocation";
    /** Indicates the message is a StreamItem message and implements the {@link StreamItemMessage} interface. */
    MessageType[MessageType["StreamItem"] = 2] = "StreamItem";
    /** Indicates the message is a Completion message and implements the {@link CompletionMessage} interface. */
    MessageType[MessageType["Completion"] = 3] = "Completion";
    /** Indicates the message is a Stream Invocation message and implements the {@link StreamInvocationMessage} interface. */
    MessageType[MessageType["StreamInvocation"] = 4] = "StreamInvocation";
    /** Indicates the message is a Cancel Invocation message and implements the {@link CancelInvocationMessage} interface. */
    MessageType[MessageType["CancelInvocation"] = 5] = "CancelInvocation";
    /** Indicates the message is a Ping message and implements the {@link PingMessage} interface. */
    MessageType[MessageType["Ping"] = 6] = "Ping";
    /** Indicates the message is a Close message and implements the {@link CloseMessage} interface. */
    MessageType[MessageType["Close"] = 7] = "Close";
})(MessageType || (MessageType = {}));

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
/** A logger that does nothing when log messages are sent to it. */
var NullLogger = /** @class */ (function () {
    function NullLogger() {
    }
    /** @inheritDoc */
    NullLogger.prototype.log = function (logLevel, message) {
    };
    /** The singleton instance of the {@link NullLogger}. */
    NullLogger.instance = new NullLogger();
    return NullLogger;
}());

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var Arg = /** @class */ (function () {
    function Arg() {
    }
    Arg.isRequired = function (val, name) {
        if (val === null || val === undefined) {
            throw new Error("The '" + name + "' argument is required.");
        }
    };
    Arg.isIn = function (val, values, name) {
        // TypeScript enums have keys for **both** the name and the value of each enum member on the type itself.
        if (!(val in values)) {
            throw new Error("Unknown " + name + " value: " + val + ".");
        }
    };
    return Arg;
}());
function getDataDetail(data, includeContent) {
    var detail = "";
    if (data instanceof ArrayBuffer) {
        detail = "Binary data of length " + data.byteLength;
        if (includeContent) {
            detail += ". Content: '" + formatArrayBuffer(data) + "'";
        }
    }
    else if (typeof data === "string") {
        detail = "String data of length " + data.length;
        if (includeContent) {
            detail += ". Content: '" + data + "'.";
        }
    }
    return detail;
}
function formatArrayBuffer(data) {
    var view = new Uint8Array(data);
    // Uint8Array.map only supports returning another Uint8Array?
    var str = "";
    view.forEach(function (num) {
        var pad = num < 16 ? "0" : "";
        str += "0x" + pad + num.toString(16) + " ";
    });
    // Trim of trailing space.
    return str.substr(0, str.length - 1);
}
function sendMessage(logger, transportName, httpClient, url, accessTokenFactory, content, logMessageContent) {
    return tslib_1.__awaiter(this, void 0, void 0, function () {
        var headers, token, response, _a;
        return tslib_1.__generator(this, function (_b) {
            switch (_b.label) {
                case 0:
                    if (!accessTokenFactory) return [3 /*break*/, 2];
                    return [4 /*yield*/, accessTokenFactory()];
                case 1:
                    token = _b.sent();
                    if (token) {
                        headers = (_a = {}, _a["Authorization"] = "Bearer " + token, _a);
                    }
                    _b.label = 2;
                case 2:
                    logger.log(LogLevel.Trace, "(" + transportName + " transport) sending data. " + getDataDetail(content, logMessageContent) + ".");
                    return [4 /*yield*/, httpClient.post(url, {
                            content: content,
                            headers: headers,
                        })];
                case 3:
                    response = _b.sent();
                    logger.log(LogLevel.Trace, "(" + transportName + " transport) request complete. Response status: " + response.statusCode + ".");
                    return [2 /*return*/];
            }
        });
    });
}
function createLogger(logger) {
    if (logger === undefined) {
        return new ConsoleLogger(LogLevel.Information);
    }
    if (logger === null) {
        return NullLogger.instance;
    }
    if (logger.log) {
        return logger;
    }
    return new ConsoleLogger(logger);
}
var Subject = /** @class */ (function () {
    function Subject(cancelCallback) {
        this.observers = [];
        this.cancelCallback = cancelCallback;
    }
    Subject.prototype.next = function (item) {
        for (var _i = 0, _a = this.observers; _i < _a.length; _i++) {
            var observer = _a[_i];
            observer.next(item);
        }
    };
    Subject.prototype.error = function (err) {
        for (var _i = 0, _a = this.observers; _i < _a.length; _i++) {
            var observer = _a[_i];
            if (observer.error) {
                observer.error(err);
            }
        }
    };
    Subject.prototype.complete = function () {
        for (var _i = 0, _a = this.observers; _i < _a.length; _i++) {
            var observer = _a[_i];
            if (observer.complete) {
                observer.complete();
            }
        }
    };
    Subject.prototype.subscribe = function (observer) {
        this.observers.push(observer);
        return new SubjectSubscription(this, observer);
    };
    return Subject;
}());
var SubjectSubscription = /** @class */ (function () {
    function SubjectSubscription(subject, observer) {
        this.subject = subject;
        this.observer = observer;
    }
    SubjectSubscription.prototype.dispose = function () {
        var index = this.subject.observers.indexOf(this.observer);
        if (index > -1) {
            this.subject.observers.splice(index, 1);
        }
        if (this.subject.observers.length === 0) {
            this.subject.cancelCallback().catch(function (_) { });
        }
    };
    return SubjectSubscription;
}());
var ConsoleLogger = /** @class */ (function () {
    function ConsoleLogger(minimumLogLevel) {
        this.minimumLogLevel = minimumLogLevel;
    }
    ConsoleLogger.prototype.log = function (logLevel, message) {
        if (logLevel >= this.minimumLogLevel) {
            switch (logLevel) {
                case LogLevel.Critical:
                case LogLevel.Error:
                    console.error(LogLevel[logLevel] + ": " + message);
                    break;
                case LogLevel.Warning:
                    console.warn(LogLevel[logLevel] + ": " + message);
                    break;
                case LogLevel.Information:
                    console.info(LogLevel[logLevel] + ": " + message);
                    break;
                default:
                    // console.debug only goes to attached debuggers in Node, so we use console.log for Trace and Debug
                    console.log(LogLevel[logLevel] + ": " + message);
                    break;
            }
        }
    };
    return ConsoleLogger;
}());

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var DEFAULT_TIMEOUT_IN_MS = 30 * 1000;
var DEFAULT_PING_INTERVAL_IN_MS = 15 * 1000;
/** Describes the current state of the {@link HubConnection} to the server. */
var HubConnectionState;
(function (HubConnectionState) {
    /** The hub connection is disconnected. */
    HubConnectionState[HubConnectionState["Disconnected"] = 0] = "Disconnected";
    /** The hub connection is connected. */
    HubConnectionState[HubConnectionState["Connected"] = 1] = "Connected";
})(HubConnectionState || (HubConnectionState = {}));
/** Represents a connection to a SignalR Hub. */
var HubConnection = /** @class */ (function () {
    function HubConnection(connection, logger, protocol) {
        var _this = this;
        Arg.isRequired(connection, "connection");
        Arg.isRequired(logger, "logger");
        Arg.isRequired(protocol, "protocol");
        this.serverTimeoutInMilliseconds = DEFAULT_TIMEOUT_IN_MS;
        this.keepAliveIntervalInMilliseconds = DEFAULT_PING_INTERVAL_IN_MS;
        this.logger = logger;
        this.protocol = protocol;
        this.connection = connection;
        this.handshakeProtocol = new HandshakeProtocol();
        this.connection.onreceive = function (data) { return _this.processIncomingData(data); };
        this.connection.onclose = function (error) { return _this.connectionClosed(error); };
        this.callbacks = {};
        this.methods = {};
        this.closedCallbacks = [];
        this.id = 0;
        this.receivedHandshakeResponse = false;
        this.connectionState = HubConnectionState.Disconnected;
        this.cachedPingMessage = this.protocol.writeMessage({ type: MessageType.Ping });
    }
    /** @internal */
    // Using a public static factory method means we can have a private constructor and an _internal_
    // create method that can be used by HubConnectionBuilder. An "internal" constructor would just
    // be stripped away and the '.d.ts' file would have no constructor, which is interpreted as a
    // public parameter-less constructor.
    HubConnection.create = function (connection, logger, protocol) {
        return new HubConnection(connection, logger, protocol);
    };
    Object.defineProperty(HubConnection.prototype, "state", {
        /** Indicates the state of the {@link HubConnection} to the server. */
        get: function () {
            return this.connectionState;
        },
        enumerable: true,
        configurable: true
    });
    /** Starts the connection.
     *
     * @returns {Promise<void>} A Promise that resolves when the connection has been successfully established, or rejects with an error.
     */
    HubConnection.prototype.start = function () {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var handshakeRequest;
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        handshakeRequest = {
                            protocol: this.protocol.name,
                            version: this.protocol.version,
                        };
                        this.logger.log(LogLevel.Debug, "Starting HubConnection.");
                        this.receivedHandshakeResponse = false;
                        return [4 /*yield*/, this.connection.start(this.protocol.transferFormat)];
                    case 1:
                        _a.sent();
                        this.logger.log(LogLevel.Debug, "Sending handshake request.");
                        return [4 /*yield*/, this.sendMessage(this.handshakeProtocol.writeHandshakeRequest(handshakeRequest))];
                    case 2:
                        _a.sent();
                        this.logger.log(LogLevel.Information, "Using HubProtocol '" + this.protocol.name + "'.");
                        // defensively cleanup timeout in case we receive a message from the server before we finish start
                        this.cleanupTimeout();
                        this.resetTimeoutPeriod();
                        this.resetKeepAliveInterval();
                        this.connectionState = HubConnectionState.Connected;
                        return [2 /*return*/];
                }
            });
        });
    };
    /** Stops the connection.
     *
     * @returns {Promise<void>} A Promise that resolves when the connection has been successfully terminated, or rejects with an error.
     */
    HubConnection.prototype.stop = function () {
        this.logger.log(LogLevel.Debug, "Stopping HubConnection.");
        this.cleanupTimeout();
        this.cleanupPingTimer();
        return this.connection.stop();
    };
    /** Invokes a streaming hub method on the server using the specified name and arguments.
     *
     * @typeparam T The type of the items returned by the server.
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {IStreamResult<T>} An object that yields results from the server as they are received.
     */
    HubConnection.prototype.stream = function (methodName) {
        var _this = this;
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        var invocationDescriptor = this.createStreamInvocation(methodName, args);
        var subject = new Subject(function () {
            var cancelInvocation = _this.createCancelInvocation(invocationDescriptor.invocationId);
            var cancelMessage = _this.protocol.writeMessage(cancelInvocation);
            delete _this.callbacks[invocationDescriptor.invocationId];
            return _this.sendMessage(cancelMessage);
        });
        this.callbacks[invocationDescriptor.invocationId] = function (invocationEvent, error) {
            if (error) {
                subject.error(error);
                return;
            }
            else if (invocationEvent) {
                // invocationEvent will not be null when an error is not passed to the callback
                if (invocationEvent.type === MessageType.Completion) {
                    if (invocationEvent.error) {
                        subject.error(new Error(invocationEvent.error));
                    }
                    else {
                        subject.complete();
                    }
                }
                else {
                    subject.next((invocationEvent.item));
                }
            }
        };
        var message = this.protocol.writeMessage(invocationDescriptor);
        this.sendMessage(message)
            .catch(function (e) {
            subject.error(e);
            delete _this.callbacks[invocationDescriptor.invocationId];
        });
        return subject;
    };
    HubConnection.prototype.sendMessage = function (message) {
        this.resetKeepAliveInterval();
        return this.connection.send(message);
    };
    /** Invokes a hub method on the server using the specified name and arguments. Does not wait for a response from the receiver.
     *
     * The Promise returned by this method resolves when the client has sent the invocation to the server. The server may still
     * be processing the invocation.
     *
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {Promise<void>} A Promise that resolves when the invocation has been successfully sent, or rejects with an error.
     */
    HubConnection.prototype.send = function (methodName) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        var invocationDescriptor = this.createInvocation(methodName, args, true);
        var message = this.protocol.writeMessage(invocationDescriptor);
        return this.sendMessage(message);
    };
    /** Invokes a hub method on the server using the specified name and arguments.
     *
     * The Promise returned by this method resolves when the server indicates it has finished invoking the method. When the promise
     * resolves, the server has finished invoking the method. If the server method returns a result, it is produced as the result of
     * resolving the Promise.
     *
     * @typeparam T The expected return type.
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {Promise<T>} A Promise that resolves with the result of the server method (if any), or rejects with an error.
     */
    HubConnection.prototype.invoke = function (methodName) {
        var _this = this;
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        var invocationDescriptor = this.createInvocation(methodName, args, false);
        var p = new Promise(function (resolve, reject) {
            // invocationId will always have a value for a non-blocking invocation
            _this.callbacks[invocationDescriptor.invocationId] = function (invocationEvent, error) {
                if (error) {
                    reject(error);
                    return;
                }
                else if (invocationEvent) {
                    // invocationEvent will not be null when an error is not passed to the callback
                    if (invocationEvent.type === MessageType.Completion) {
                        if (invocationEvent.error) {
                            reject(new Error(invocationEvent.error));
                        }
                        else {
                            resolve(invocationEvent.result);
                        }
                    }
                    else {
                        reject(new Error("Unexpected message type: " + invocationEvent.type));
                    }
                }
            };
            var message = _this.protocol.writeMessage(invocationDescriptor);
            _this.sendMessage(message)
                .catch(function (e) {
                reject(e);
                // invocationId will always have a value for a non-blocking invocation
                delete _this.callbacks[invocationDescriptor.invocationId];
            });
        });
        return p;
    };
    /** Registers a handler that will be invoked when the hub method with the specified method name is invoked.
     *
     * @param {string} methodName The name of the hub method to define.
     * @param {Function} newMethod The handler that will be raised when the hub method is invoked.
     */
    HubConnection.prototype.on = function (methodName, newMethod) {
        if (!methodName || !newMethod) {
            return;
        }
        methodName = methodName.toLowerCase();
        if (!this.methods[methodName]) {
            this.methods[methodName] = [];
        }
        // Preventing adding the same handler multiple times.
        if (this.methods[methodName].indexOf(newMethod) !== -1) {
            return;
        }
        this.methods[methodName].push(newMethod);
    };
    HubConnection.prototype.off = function (methodName, method) {
        if (!methodName) {
            return;
        }
        methodName = methodName.toLowerCase();
        var handlers = this.methods[methodName];
        if (!handlers) {
            return;
        }
        if (method) {
            var removeIdx = handlers.indexOf(method);
            if (removeIdx !== -1) {
                handlers.splice(removeIdx, 1);
                if (handlers.length === 0) {
                    delete this.methods[methodName];
                }
            }
        }
        else {
            delete this.methods[methodName];
        }
    };
    /** Registers a handler that will be invoked when the connection is closed.
     *
     * @param {Function} callback The handler that will be invoked when the connection is closed. Optionally receives a single argument containing the error that caused the connection to close (if any).
     */
    HubConnection.prototype.onclose = function (callback) {
        if (callback) {
            this.closedCallbacks.push(callback);
        }
    };
    HubConnection.prototype.processIncomingData = function (data) {
        this.cleanupTimeout();
        if (!this.receivedHandshakeResponse) {
            data = this.processHandshakeResponse(data);
            this.receivedHandshakeResponse = true;
        }
        // Data may have all been read when processing handshake response
        if (data) {
            // Parse the messages
            var messages = this.protocol.parseMessages(data, this.logger);
            for (var _i = 0, messages_1 = messages; _i < messages_1.length; _i++) {
                var message = messages_1[_i];
                switch (message.type) {
                    case MessageType.Invocation:
                        this.invokeClientMethod(message);
                        break;
                    case MessageType.StreamItem:
                    case MessageType.Completion:
                        var callback = this.callbacks[message.invocationId];
                        if (callback != null) {
                            if (message.type === MessageType.Completion) {
                                delete this.callbacks[message.invocationId];
                            }
                            callback(message);
                        }
                        break;
                    case MessageType.Ping:
                        // Don't care about pings
                        break;
                    case MessageType.Close:
                        this.logger.log(LogLevel.Information, "Close message received from server.");
                        this.connection.stop(message.error ? new Error("Server returned an error on close: " + message.error) : undefined);
                        break;
                    default:
                        this.logger.log(LogLevel.Warning, "Invalid message type: " + message.type);
                        break;
                }
            }
        }
        this.resetTimeoutPeriod();
    };
    HubConnection.prototype.processHandshakeResponse = function (data) {
        var responseMessage;
        var remainingData;
        try {
            _a = this.handshakeProtocol.parseHandshakeResponse(data), remainingData = _a[0], responseMessage = _a[1];
        }
        catch (e) {
            var message = "Error parsing handshake response: " + e;
            this.logger.log(LogLevel.Error, message);
            var error = new Error(message);
            this.connection.stop(error);
            throw error;
        }
        if (responseMessage.error) {
            var message = "Server returned handshake error: " + responseMessage.error;
            this.logger.log(LogLevel.Error, message);
            this.connection.stop(new Error(message));
        }
        else {
            this.logger.log(LogLevel.Debug, "Server handshake complete.");
        }
        return remainingData;
        var _a;
    };
    HubConnection.prototype.resetKeepAliveInterval = function () {
        var _this = this;
        this.cleanupPingTimer();
        this.pingServerHandle = setTimeout(function () { return _this.sendMessage(_this.cachedPingMessage); }, this.keepAliveIntervalInMilliseconds);
    };
    HubConnection.prototype.resetTimeoutPeriod = function () {
        var _this = this;
        if (!this.connection.features || !this.connection.features.inherentKeepAlive) {
            // Set the timeout timer
            this.timeoutHandle = setTimeout(function () { return _this.serverTimeout(); }, this.serverTimeoutInMilliseconds);
        }
    };
    HubConnection.prototype.serverTimeout = function () {
        // The server hasn't talked to us in a while. It doesn't like us anymore ... :(
        // Terminate the connection
        this.connection.stop(new Error("Server timeout elapsed without receiving a message from the server."));
    };
    HubConnection.prototype.invokeClientMethod = function (invocationMessage) {
        var _this = this;
        var methods = this.methods[invocationMessage.target.toLowerCase()];
        if (methods) {
            methods.forEach(function (m) { return m.apply(_this, invocationMessage.arguments); });
            if (invocationMessage.invocationId) {
                // This is not supported in v1. So we return an error to avoid blocking the server waiting for the response.
                var message = "Server requested a response, which is not supported in this version of the client.";
                this.logger.log(LogLevel.Error, message);
                this.connection.stop(new Error(message));
            }
        }
        else {
            this.logger.log(LogLevel.Warning, "No client method with the name '" + invocationMessage.target + "' found.");
        }
    };
    HubConnection.prototype.connectionClosed = function (error) {
        var _this = this;
        var callbacks = this.callbacks;
        this.callbacks = {};
        this.connectionState = HubConnectionState.Disconnected;
        Object.keys(callbacks)
            .forEach(function (key) {
            var callback = callbacks[key];
            callback(null, error ? error : new Error("Invocation canceled due to connection being closed."));
        });
        this.cleanupTimeout();
        this.cleanupPingTimer();
        this.closedCallbacks.forEach(function (c) { return c.apply(_this, [error]); });
    };
    HubConnection.prototype.cleanupPingTimer = function () {
        if (this.pingServerHandle) {
            clearTimeout(this.pingServerHandle);
        }
    };
    HubConnection.prototype.cleanupTimeout = function () {
        if (this.timeoutHandle) {
            clearTimeout(this.timeoutHandle);
        }
    };
    HubConnection.prototype.createInvocation = function (methodName, args, nonblocking) {
        if (nonblocking) {
            return {
                arguments: args,
                target: methodName,
                type: MessageType.Invocation,
            };
        }
        else {
            var id = this.id;
            this.id++;
            return {
                arguments: args,
                invocationId: id.toString(),
                target: methodName,
                type: MessageType.Invocation,
            };
        }
    };
    HubConnection.prototype.createStreamInvocation = function (methodName, args) {
        var id = this.id;
        this.id++;
        return {
            arguments: args,
            invocationId: id.toString(),
            target: methodName,
            type: MessageType.StreamInvocation,
        };
    };
    HubConnection.prototype.createCancelInvocation = function (id) {
        return {
            invocationId: id,
            type: MessageType.CancelInvocation,
        };
    };
    return HubConnection;
}());

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This will be treated as a bit flag in the future, so we keep it using power-of-two values.
/** Specifies a specific HTTP transport type. */
var HttpTransportType;
(function (HttpTransportType) {
    /** Specifies no transport preference. */
    HttpTransportType[HttpTransportType["None"] = 0] = "None";
    /** Specifies the WebSockets transport. */
    HttpTransportType[HttpTransportType["WebSockets"] = 1] = "WebSockets";
    /** Specifies the Server-Sent Events transport. */
    HttpTransportType[HttpTransportType["ServerSentEvents"] = 2] = "ServerSentEvents";
    /** Specifies the Long Polling transport. */
    HttpTransportType[HttpTransportType["LongPolling"] = 4] = "LongPolling";
})(HttpTransportType || (HttpTransportType = {}));
/** Specifies the transfer format for a connection. */
var TransferFormat;
(function (TransferFormat) {
    /** Specifies that only text data will be transmitted over the connection. */
    TransferFormat[TransferFormat["Text"] = 1] = "Text";
    /** Specifies that binary data will be transmitted over the connection. */
    TransferFormat[TransferFormat["Binary"] = 2] = "Binary";
})(TransferFormat || (TransferFormat = {}));

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Rough polyfill of https://developer.mozilla.org/en-US/docs/Web/API/AbortController
// We don't actually ever use the API being polyfilled, we always use the polyfill because
// it's a very new API right now.
// Not exported from index.
var AbortController = /** @class */ (function () {
    function AbortController() {
        this.isAborted = false;
        this.onabort = null;
    }
    AbortController.prototype.abort = function () {
        if (!this.isAborted) {
            this.isAborted = true;
            if (this.onabort) {
                this.onabort();
            }
        }
    };
    Object.defineProperty(AbortController.prototype, "signal", {
        get: function () {
            return this;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AbortController.prototype, "aborted", {
        get: function () {
            return this.isAborted;
        },
        enumerable: true,
        configurable: true
    });
    return AbortController;
}());

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Not exported from 'index', this type is internal.
var LongPollingTransport = /** @class */ (function () {
    function LongPollingTransport(httpClient, accessTokenFactory, logger, logMessageContent) {
        this.httpClient = httpClient;
        this.accessTokenFactory = accessTokenFactory;
        this.logger = logger;
        this.pollAbort = new AbortController();
        this.logMessageContent = logMessageContent;
        this.running = false;
        this.onreceive = null;
        this.onclose = null;
    }
    Object.defineProperty(LongPollingTransport.prototype, "pollAborted", {
        // This is an internal type, not exported from 'index' so this is really just internal.
        get: function () {
            return this.pollAbort.aborted;
        },
        enumerable: true,
        configurable: true
    });
    LongPollingTransport.prototype.connect = function (url, transferFormat) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var pollOptions, token, pollUrl, response;
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        Arg.isRequired(url, "url");
                        Arg.isRequired(transferFormat, "transferFormat");
                        Arg.isIn(transferFormat, TransferFormat, "transferFormat");
                        this.url = url;
                        this.logger.log(LogLevel.Trace, "(LongPolling transport) Connecting");
                        if (transferFormat === TransferFormat.Binary && (typeof new XMLHttpRequest().responseType !== "string")) {
                            // This will work if we fix: https://github.com/aspnet/SignalR/issues/742
                            throw new Error("Binary protocols over XmlHttpRequest not implementing advanced features are not supported.");
                        }
                        pollOptions = {
                            abortSignal: this.pollAbort.signal,
                            headers: {},
                            timeout: 90000,
                        };
                        if (transferFormat === TransferFormat.Binary) {
                            pollOptions.responseType = "arraybuffer";
                        }
                        return [4 /*yield*/, this.getAccessToken()];
                    case 1:
                        token = _a.sent();
                        this.updateHeaderToken(pollOptions, token);
                        pollUrl = url + "&_=" + Date.now();
                        this.logger.log(LogLevel.Trace, "(LongPolling transport) polling: " + pollUrl);
                        return [4 /*yield*/, this.httpClient.get(pollUrl, pollOptions)];
                    case 2:
                        response = _a.sent();
                        if (response.statusCode !== 200) {
                            this.logger.log(LogLevel.Error, "(LongPolling transport) Unexpected response code: " + response.statusCode);
                            // Mark running as false so that the poll immediately ends and runs the close logic
                            this.closeError = new HttpError(response.statusText || "", response.statusCode);
                            this.running = false;
                        }
                        else {
                            this.running = true;
                        }
                        this.receiving = this.poll(this.url, pollOptions);
                        return [2 /*return*/];
                }
            });
        });
    };
    LongPollingTransport.prototype.getAccessToken = function () {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!this.accessTokenFactory) return [3 /*break*/, 2];
                        return [4 /*yield*/, this.accessTokenFactory()];
                    case 1: return [2 /*return*/, _a.sent()];
                    case 2: return [2 /*return*/, null];
                }
            });
        });
    };
    LongPollingTransport.prototype.updateHeaderToken = function (request, token) {
        if (!request.headers) {
            request.headers = {};
        }
        if (token) {
            // tslint:disable-next-line:no-string-literal
            request.headers["Authorization"] = "Bearer " + token;
            return;
        }
        // tslint:disable-next-line:no-string-literal
        if (request.headers["Authorization"]) {
            // tslint:disable-next-line:no-string-literal
            delete request.headers["Authorization"];
        }
    };
    LongPollingTransport.prototype.poll = function (url, pollOptions) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var token, pollUrl, response, e_1;
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, , 8, 9]);
                        _a.label = 1;
                    case 1:
                        if (!this.running) return [3 /*break*/, 7];
                        return [4 /*yield*/, this.getAccessToken()];
                    case 2:
                        token = _a.sent();
                        this.updateHeaderToken(pollOptions, token);
                        _a.label = 3;
                    case 3:
                        _a.trys.push([3, 5, , 6]);
                        pollUrl = url + "&_=" + Date.now();
                        this.logger.log(LogLevel.Trace, "(LongPolling transport) polling: " + pollUrl);
                        return [4 /*yield*/, this.httpClient.get(pollUrl, pollOptions)];
                    case 4:
                        response = _a.sent();
                        if (response.statusCode === 204) {
                            this.logger.log(LogLevel.Information, "(LongPolling transport) Poll terminated by server");
                            this.running = false;
                        }
                        else if (response.statusCode !== 200) {
                            this.logger.log(LogLevel.Error, "(LongPolling transport) Unexpected response code: " + response.statusCode);
                            // Unexpected status code
                            this.closeError = new HttpError(response.statusText || "", response.statusCode);
                            this.running = false;
                        }
                        else {
                            // Process the response
                            if (response.content) {
                                this.logger.log(LogLevel.Trace, "(LongPolling transport) data received. " + getDataDetail(response.content, this.logMessageContent));
                                if (this.onreceive) {
                                    this.onreceive(response.content);
                                }
                            }
                            else {
                                // This is another way timeout manifest.
                                this.logger.log(LogLevel.Trace, "(LongPolling transport) Poll timed out, reissuing.");
                            }
                        }
                        return [3 /*break*/, 6];
                    case 5:
                        e_1 = _a.sent();
                        if (!this.running) {
                            // Log but disregard errors that occur after stopping
                            this.logger.log(LogLevel.Trace, "(LongPolling transport) Poll errored after shutdown: " + e_1.message);
                        }
                        else {
                            if (e_1 instanceof TimeoutError) {
                                // Ignore timeouts and reissue the poll.
                                this.logger.log(LogLevel.Trace, "(LongPolling transport) Poll timed out, reissuing.");
                            }
                            else {
                                // Close the connection with the error as the result.
                                this.closeError = e_1;
                                this.running = false;
                            }
                        }
                        return [3 /*break*/, 6];
                    case 6: return [3 /*break*/, 1];
                    case 7: return [3 /*break*/, 9];
                    case 8:
                        this.logger.log(LogLevel.Trace, "(LongPolling transport) Polling complete.");
                        // We will reach here with pollAborted==false when the server returned a response causing the transport to stop.
                        // If pollAborted==true then client initiated the stop and the stop method will raise the close event after DELETE is sent.
                        if (!this.pollAborted) {
                            this.raiseOnClose();
                        }
                        return [7 /*endfinally*/];
                    case 9: return [2 /*return*/];
                }
            });
        });
    };
    LongPollingTransport.prototype.send = function (data) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            return tslib_1.__generator(this, function (_a) {
                if (!this.running) {
                    return [2 /*return*/, Promise.reject(new Error("Cannot send until the transport is connected"))];
                }
                return [2 /*return*/, sendMessage(this.logger, "LongPolling", this.httpClient, this.url, this.accessTokenFactory, data, this.logMessageContent)];
            });
        });
    };
    LongPollingTransport.prototype.stop = function () {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var deleteOptions, token;
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.logger.log(LogLevel.Trace, "(LongPolling transport) Stopping polling.");
                        // Tell receiving loop to stop, abort any current request, and then wait for it to finish
                        this.running = false;
                        this.pollAbort.abort();
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, , 5, 6]);
                        return [4 /*yield*/, this.receiving];
                    case 2:
                        _a.sent();
                        // Send DELETE to clean up long polling on the server
                        this.logger.log(LogLevel.Trace, "(LongPolling transport) sending DELETE request to " + this.url + ".");
                        deleteOptions = {
                            headers: {},
                        };
                        return [4 /*yield*/, this.getAccessToken()];
                    case 3:
                        token = _a.sent();
                        this.updateHeaderToken(deleteOptions, token);
                        return [4 /*yield*/, this.httpClient.delete(this.url, deleteOptions)];
                    case 4:
                        _a.sent();
                        this.logger.log(LogLevel.Trace, "(LongPolling transport) DELETE request sent.");
                        return [3 /*break*/, 6];
                    case 5:
                        this.logger.log(LogLevel.Trace, "(LongPolling transport) Stop finished.");
                        // Raise close event here instead of in polling
                        // It needs to happen after the DELETE request is sent
                        this.raiseOnClose();
                        return [7 /*endfinally*/];
                    case 6: return [2 /*return*/];
                }
            });
        });
    };
    LongPollingTransport.prototype.raiseOnClose = function () {
        if (this.onclose) {
            var logMessage = "(LongPolling transport) Firing onclose event.";
            if (this.closeError) {
                logMessage += " Error: " + this.closeError;
            }
            this.logger.log(LogLevel.Trace, logMessage);
            this.onclose(this.closeError);
        }
    };
    return LongPollingTransport;
}());

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var ServerSentEventsTransport = /** @class */ (function () {
    function ServerSentEventsTransport(httpClient, accessTokenFactory, logger, logMessageContent, eventSourceConstructor) {
        this.httpClient = httpClient;
        this.accessTokenFactory = accessTokenFactory;
        this.logger = logger;
        this.logMessageContent = logMessageContent;
        this.eventSourceConstructor = eventSourceConstructor;
        this.onreceive = null;
        this.onclose = null;
    }
    ServerSentEventsTransport.prototype.connect = function (url, transferFormat) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var _this = this;
            var token;
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        Arg.isRequired(url, "url");
                        Arg.isRequired(transferFormat, "transferFormat");
                        Arg.isIn(transferFormat, TransferFormat, "transferFormat");
                        this.logger.log(LogLevel.Trace, "(SSE transport) Connecting");
                        if (!this.accessTokenFactory) return [3 /*break*/, 2];
                        return [4 /*yield*/, this.accessTokenFactory()];
                    case 1:
                        token = _a.sent();
                        if (token) {
                            url += (url.indexOf("?") < 0 ? "?" : "&") + ("access_token=" + encodeURIComponent(token));
                        }
                        _a.label = 2;
                    case 2:
                        this.url = url;
                        return [2 /*return*/, new Promise(function (resolve, reject) {
                                var opened = false;
                                if (transferFormat !== TransferFormat.Text) {
                                    reject(new Error("The Server-Sent Events transport only supports the 'Text' transfer format"));
                                }
                                var eventSource = new _this.eventSourceConstructor(url, { withCredentials: true });
                                try {
                                    eventSource.onmessage = function (e) {
                                        if (_this.onreceive) {
                                            try {
                                                _this.logger.log(LogLevel.Trace, "(SSE transport) data received. " + getDataDetail(e.data, _this.logMessageContent) + ".");
                                                _this.onreceive(e.data);
                                            }
                                            catch (error) {
                                                if (_this.onclose) {
                                                    _this.onclose(error);
                                                }
                                                return;
                                            }
                                        }
                                    };
                                    eventSource.onerror = function (e) {
                                        var error = new Error(e.message || "Error occurred");
                                        if (opened) {
                                            _this.close(error);
                                        }
                                        else {
                                            reject(error);
                                        }
                                    };
                                    eventSource.onopen = function () {
                                        _this.logger.log(LogLevel.Information, "SSE connected to " + _this.url);
                                        _this.eventSource = eventSource;
                                        opened = true;
                                        resolve();
                                    };
                                }
                                catch (e) {
                                    return Promise.reject(e);
                                }
                            })];
                }
            });
        });
    };
    ServerSentEventsTransport.prototype.send = function (data) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            return tslib_1.__generator(this, function (_a) {
                if (!this.eventSource) {
                    return [2 /*return*/, Promise.reject(new Error("Cannot send until the transport is connected"))];
                }
                return [2 /*return*/, sendMessage(this.logger, "SSE", this.httpClient, this.url, this.accessTokenFactory, data, this.logMessageContent)];
            });
        });
    };
    ServerSentEventsTransport.prototype.stop = function () {
        this.close();
        return Promise.resolve();
    };
    ServerSentEventsTransport.prototype.close = function (e) {
        if (this.eventSource) {
            this.eventSource.close();
            this.eventSource = undefined;
            if (this.onclose) {
                this.onclose(e);
            }
        }
    };
    return ServerSentEventsTransport;
}());

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var WebSocketTransport = /** @class */ (function () {
    function WebSocketTransport(accessTokenFactory, logger, logMessageContent, webSocketConstructor) {
        this.logger = logger;
        this.accessTokenFactory = accessTokenFactory;
        this.logMessageContent = logMessageContent;
        this.webSocketConstructor = webSocketConstructor;
        this.onreceive = null;
        this.onclose = null;
    }
    WebSocketTransport.prototype.connect = function (url, transferFormat) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var _this = this;
            var token;
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        Arg.isRequired(url, "url");
                        Arg.isRequired(transferFormat, "transferFormat");
                        Arg.isIn(transferFormat, TransferFormat, "transferFormat");
                        this.logger.log(LogLevel.Trace, "(WebSockets transport) Connecting");
                        if (!this.accessTokenFactory) return [3 /*break*/, 2];
                        return [4 /*yield*/, this.accessTokenFactory()];
                    case 1:
                        token = _a.sent();
                        if (token) {
                            url += (url.indexOf("?") < 0 ? "?" : "&") + ("access_token=" + encodeURIComponent(token));
                        }
                        _a.label = 2;
                    case 2: return [2 /*return*/, new Promise(function (resolve, reject) {
                            url = url.replace(/^http/, "ws");
                            var webSocket = new _this.webSocketConstructor(url);
                            if (transferFormat === TransferFormat.Binary) {
                                webSocket.binaryType = "arraybuffer";
                            }
                            webSocket.onopen = function (event) {
                                _this.logger.log(LogLevel.Information, "WebSocket connected to " + url);
                                _this.webSocket = webSocket;
                                resolve();
                            };
                            webSocket.onerror = function (event) {
                                var error = (event instanceof ErrorEvent) ? event.error : null;
                                reject(error);
                            };
                            webSocket.onmessage = function (message) {
                                _this.logger.log(LogLevel.Trace, "(WebSockets transport) data received. " + getDataDetail(message.data, _this.logMessageContent) + ".");
                                if (_this.onreceive) {
                                    _this.onreceive(message.data);
                                }
                            };
                            webSocket.onclose = function (event) {
                                // webSocket will be null if the transport did not start successfully
                                _this.logger.log(LogLevel.Trace, "(WebSockets transport) socket closed.");
                                if (_this.onclose) {
                                    if (event.wasClean === false || event.code !== 1000) {
                                        _this.onclose(new Error("Websocket closed with status code: " + event.code + " (" + event.reason + ")"));
                                    }
                                    else {
                                        _this.onclose();
                                    }
                                }
                            };
                        })];
                }
            });
        });
    };
    WebSocketTransport.prototype.send = function (data) {
        if (this.webSocket && this.webSocket.readyState === WebSocket.OPEN) {
            this.logger.log(LogLevel.Trace, "(WebSockets transport) sending data. " + getDataDetail(data, this.logMessageContent) + ".");
            this.webSocket.send(data);
            return Promise.resolve();
        }
        return Promise.reject("WebSocket is not in the OPEN state");
    };
    WebSocketTransport.prototype.stop = function () {
        if (this.webSocket) {
            this.webSocket.close();
            this.webSocket = undefined;
        }
        return Promise.resolve();
    };
    return WebSocketTransport;
}());

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var MAX_REDIRECTS = 100;
var HttpConnection = /** @class */ (function () {
    function HttpConnection(url, options) {
        if (options === void 0) { options = {}; }
        this.features = {};
        Arg.isRequired(url, "url");
        this.logger = createLogger(options.logger);
        this.baseUrl = this.resolveUrl(url);
        options = options || {};
        options.logMessageContent = options.logMessageContent || false;
        if (typeof WebSocket !== "undefined" && !options.WebSocket) {
            options.WebSocket = WebSocket;
        }
        if (typeof EventSource !== "undefined" && !options.EventSource) {
            options.EventSource = EventSource;
        }
        this.httpClient = options.httpClient || new DefaultHttpClient(this.logger);
        this.connectionState = 2 /* Disconnected */;
        this.options = options;
        this.onreceive = null;
        this.onclose = null;
    }
    HttpConnection.prototype.start = function (transferFormat) {
        transferFormat = transferFormat || TransferFormat.Binary;
        Arg.isIn(transferFormat, TransferFormat, "transferFormat");
        this.logger.log(LogLevel.Debug, "Starting connection with transfer format '" + TransferFormat[transferFormat] + "'.");
        if (this.connectionState !== 2 /* Disconnected */) {
            return Promise.reject(new Error("Cannot start a connection that is not in the 'Disconnected' state."));
        }
        this.connectionState = 0 /* Connecting */;
        this.startPromise = this.startInternal(transferFormat);
        return this.startPromise;
    };
    HttpConnection.prototype.send = function (data) {
        if (this.connectionState !== 1 /* Connected */) {
            throw new Error("Cannot send data if the connection is not in the 'Connected' State.");
        }
        // Transport will not be null if state is connected
        return this.transport.send(data);
    };
    HttpConnection.prototype.stop = function (error) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var e_1;
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.connectionState = 2 /* Disconnected */;
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 3, , 4]);
                        return [4 /*yield*/, this.startPromise];
                    case 2:
                        _a.sent();
                        return [3 /*break*/, 4];
                    case 3:
                        e_1 = _a.sent();
                        return [3 /*break*/, 4];
                    case 4:
                        if (!this.transport) return [3 /*break*/, 6];
                        this.stopError = error;
                        return [4 /*yield*/, this.transport.stop()];
                    case 5:
                        _a.sent();
                        this.transport = undefined;
                        _a.label = 6;
                    case 6: return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype.startInternal = function (transferFormat) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var _this = this;
            var url, negotiateResponse, redirects, _loop_1, this_1, state_1, e_2;
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        url = this.baseUrl;
                        this.accessTokenFactory = this.options.accessTokenFactory;
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 12, , 13]);
                        if (!this.options.skipNegotiation) return [3 /*break*/, 5];
                        if (!(this.options.transport === HttpTransportType.WebSockets)) return [3 /*break*/, 3];
                        // No need to add a connection ID in this case
                        this.transport = this.constructTransport(HttpTransportType.WebSockets);
                        // We should just call connect directly in this case.
                        // No fallback or negotiate in this case.
                        return [4 /*yield*/, this.transport.connect(url, transferFormat)];
                    case 2:
                        // We should just call connect directly in this case.
                        // No fallback or negotiate in this case.
                        _a.sent();
                        return [3 /*break*/, 4];
                    case 3: throw Error("Negotiation can only be skipped when using the WebSocket transport directly.");
                    case 4: return [3 /*break*/, 11];
                    case 5:
                        negotiateResponse = null;
                        redirects = 0;
                        _loop_1 = function () {
                            var accessToken_1;
                            return tslib_1.__generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, this_1.getNegotiationResponse(url)];
                                    case 1:
                                        negotiateResponse = _a.sent();
                                        // the user tries to stop the connection when it is being started
                                        if (this_1.connectionState === 2 /* Disconnected */) {
                                            return [2 /*return*/, { value: void 0 }];
                                        }
                                        if (negotiateResponse.url) {
                                            url = negotiateResponse.url;
                                        }
                                        if (negotiateResponse.accessToken) {
                                            accessToken_1 = negotiateResponse.accessToken;
                                            this_1.accessTokenFactory = function () { return accessToken_1; };
                                        }
                                        redirects++;
                                        return [2 /*return*/];
                                }
                            });
                        };
                        this_1 = this;
                        _a.label = 6;
                    case 6: return [5 /*yield**/, _loop_1()];
                    case 7:
                        state_1 = _a.sent();
                        if (typeof state_1 === "object")
                            return [2 /*return*/, state_1.value];
                        _a.label = 8;
                    case 8:
                        if (negotiateResponse.url && redirects < MAX_REDIRECTS) return [3 /*break*/, 6];
                        _a.label = 9;
                    case 9:
                        if (redirects === MAX_REDIRECTS && negotiateResponse.url) {
                            throw Error("Negotiate redirection limit exceeded.");
                        }
                        return [4 /*yield*/, this.createTransport(url, this.options.transport, negotiateResponse, transferFormat)];
                    case 10:
                        _a.sent();
                        _a.label = 11;
                    case 11:
                        if (this.transport instanceof LongPollingTransport) {
                            this.features.inherentKeepAlive = true;
                        }
                        this.transport.onreceive = this.onreceive;
                        this.transport.onclose = function (e) { return _this.stopConnection(e); };
                        // only change the state if we were connecting to not overwrite
                        // the state if the connection is already marked as Disconnected
                        this.changeState(0 /* Connecting */, 1 /* Connected */);
                        return [3 /*break*/, 13];
                    case 12:
                        e_2 = _a.sent();
                        this.logger.log(LogLevel.Error, "Failed to start the connection: " + e_2);
                        this.connectionState = 2 /* Disconnected */;
                        this.transport = undefined;
                        throw e_2;
                    case 13: return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype.getNegotiationResponse = function (url) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var headers, token, negotiateUrl, response, e_3, _a;
            return tslib_1.__generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        if (!this.accessTokenFactory) return [3 /*break*/, 2];
                        return [4 /*yield*/, this.accessTokenFactory()];
                    case 1:
                        token = _b.sent();
                        if (token) {
                            headers = (_a = {}, _a["Authorization"] = "Bearer " + token, _a);
                        }
                        _b.label = 2;
                    case 2:
                        negotiateUrl = this.resolveNegotiateUrl(url);
                        this.logger.log(LogLevel.Debug, "Sending negotiation request: " + negotiateUrl);
                        _b.label = 3;
                    case 3:
                        _b.trys.push([3, 5, , 6]);
                        return [4 /*yield*/, this.httpClient.post(negotiateUrl, {
                                content: "",
                                headers: headers,
                            })];
                    case 4:
                        response = _b.sent();
                        if (response.statusCode !== 200) {
                            throw Error("Unexpected status code returned from negotiate " + response.statusCode);
                        }
                        return [2 /*return*/, JSON.parse(response.content)];
                    case 5:
                        e_3 = _b.sent();
                        this.logger.log(LogLevel.Error, "Failed to complete negotiation with the server: " + e_3);
                        throw e_3;
                    case 6: return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype.createConnectUrl = function (url, connectionId) {
        if (!connectionId) {
            return url;
        }
        return url + (url.indexOf("?") === -1 ? "?" : "&") + ("id=" + connectionId);
    };
    HttpConnection.prototype.createTransport = function (url, requestedTransport, negotiateResponse, requestedTransferFormat) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            var connectUrl, transports, _i, transports_1, endpoint, transport, ex_1;
            return tslib_1.__generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        connectUrl = this.createConnectUrl(url, negotiateResponse.connectionId);
                        if (!this.isITransport(requestedTransport)) return [3 /*break*/, 2];
                        this.logger.log(LogLevel.Debug, "Connection was provided an instance of ITransport, using that directly.");
                        this.transport = requestedTransport;
                        return [4 /*yield*/, this.transport.connect(connectUrl, requestedTransferFormat)];
                    case 1:
                        _a.sent();
                        // only change the state if we were connecting to not overwrite
                        // the state if the connection is already marked as Disconnected
                        this.changeState(0 /* Connecting */, 1 /* Connected */);
                        return [2 /*return*/];
                    case 2:
                        transports = negotiateResponse.availableTransports || [];
                        _i = 0, transports_1 = transports;
                        _a.label = 3;
                    case 3:
                        if (!(_i < transports_1.length)) return [3 /*break*/, 9];
                        endpoint = transports_1[_i];
                        this.connectionState = 0 /* Connecting */;
                        transport = this.resolveTransport(endpoint, requestedTransport, requestedTransferFormat);
                        if (!(typeof transport === "number")) return [3 /*break*/, 8];
                        this.transport = this.constructTransport(transport);
                        if (!!negotiateResponse.connectionId) return [3 /*break*/, 5];
                        return [4 /*yield*/, this.getNegotiationResponse(url)];
                    case 4:
                        negotiateResponse = _a.sent();
                        connectUrl = this.createConnectUrl(url, negotiateResponse.connectionId);
                        _a.label = 5;
                    case 5:
                        _a.trys.push([5, 7, , 8]);
                        return [4 /*yield*/, this.transport.connect(connectUrl, requestedTransferFormat)];
                    case 6:
                        _a.sent();
                        this.changeState(0 /* Connecting */, 1 /* Connected */);
                        return [2 /*return*/];
                    case 7:
                        ex_1 = _a.sent();
                        this.logger.log(LogLevel.Error, "Failed to start the transport '" + HttpTransportType[transport] + "': " + ex_1);
                        this.connectionState = 2 /* Disconnected */;
                        negotiateResponse.connectionId = undefined;
                        return [3 /*break*/, 8];
                    case 8:
                        _i++;
                        return [3 /*break*/, 3];
                    case 9: throw new Error("Unable to initialize any of the available transports.");
                }
            });
        });
    };
    HttpConnection.prototype.constructTransport = function (transport) {
        switch (transport) {
            case HttpTransportType.WebSockets:
                if (!this.options.WebSocket) {
                    throw new Error("'WebSocket' is not supported in your environment.");
                }
                return new WebSocketTransport(this.accessTokenFactory, this.logger, this.options.logMessageContent || false, this.options.WebSocket);
            case HttpTransportType.ServerSentEvents:
                if (!this.options.EventSource) {
                    throw new Error("'EventSource' is not supported in your environment.");
                }
                return new ServerSentEventsTransport(this.httpClient, this.accessTokenFactory, this.logger, this.options.logMessageContent || false, this.options.EventSource);
            case HttpTransportType.LongPolling:
                return new LongPollingTransport(this.httpClient, this.accessTokenFactory, this.logger, this.options.logMessageContent || false);
            default:
                throw new Error("Unknown transport: " + transport + ".");
        }
    };
    HttpConnection.prototype.resolveTransport = function (endpoint, requestedTransport, requestedTransferFormat) {
        var transport = HttpTransportType[endpoint.transport];
        if (transport === null || transport === undefined) {
            this.logger.log(LogLevel.Debug, "Skipping transport '" + endpoint.transport + "' because it is not supported by this client.");
        }
        else {
            var transferFormats = endpoint.transferFormats.map(function (s) { return TransferFormat[s]; });
            if (transportMatches(requestedTransport, transport)) {
                if (transferFormats.indexOf(requestedTransferFormat) >= 0) {
                    if ((transport === HttpTransportType.WebSockets && !this.options.WebSocket) ||
                        (transport === HttpTransportType.ServerSentEvents && !this.options.EventSource)) {
                        this.logger.log(LogLevel.Debug, "Skipping transport '" + HttpTransportType[transport] + "' because it is not supported in your environment.'");
                    }
                    else {
                        this.logger.log(LogLevel.Debug, "Selecting transport '" + HttpTransportType[transport] + "'");
                        return transport;
                    }
                }
                else {
                    this.logger.log(LogLevel.Debug, "Skipping transport '" + HttpTransportType[transport] + "' because it does not support the requested transfer format '" + TransferFormat[requestedTransferFormat] + "'.");
                }
            }
            else {
                this.logger.log(LogLevel.Debug, "Skipping transport '" + HttpTransportType[transport] + "' because it was disabled by the client.");
            }
        }
        return null;
    };
    HttpConnection.prototype.isITransport = function (transport) {
        return transport && typeof (transport) === "object" && "connect" in transport;
    };
    HttpConnection.prototype.changeState = function (from, to) {
        if (this.connectionState === from) {
            this.connectionState = to;
            return true;
        }
        return false;
    };
    HttpConnection.prototype.stopConnection = function (error) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            return tslib_1.__generator(this, function (_a) {
                this.transport = undefined;
                // If we have a stopError, it takes precedence over the error from the transport
                error = this.stopError || error;
                if (error) {
                    this.logger.log(LogLevel.Error, "Connection disconnected with error '" + error + "'.");
                }
                else {
                    this.logger.log(LogLevel.Information, "Connection disconnected.");
                }
                this.connectionState = 2 /* Disconnected */;
                if (this.onclose) {
                    this.onclose(error);
                }
                return [2 /*return*/];
            });
        });
    };
    HttpConnection.prototype.resolveUrl = function (url) {
        // startsWith is not supported in IE
        if (url.lastIndexOf("https://", 0) === 0 || url.lastIndexOf("http://", 0) === 0) {
            return url;
        }
        if (typeof window === "undefined" || !window || !window.document) {
            throw new Error("Cannot resolve '" + url + "'.");
        }
        // Setting the url to the href propery of an anchor tag handles normalization
        // for us. There are 3 main cases.
        // 1. Relative  path normalization e.g "b" -> "http://localhost:5000/a/b"
        // 2. Absolute path normalization e.g "/a/b" -> "http://localhost:5000/a/b"
        // 3. Networkpath reference normalization e.g "//localhost:5000/a/b" -> "http://localhost:5000/a/b"
        var aTag = window.document.createElement("a");
        aTag.href = url;
        this.logger.log(LogLevel.Information, "Normalizing '" + url + "' to '" + aTag.href + "'.");
        return aTag.href;
    };
    HttpConnection.prototype.resolveNegotiateUrl = function (url) {
        var index = url.indexOf("?");
        var negotiateUrl = url.substring(0, index === -1 ? url.length : index);
        if (negotiateUrl[negotiateUrl.length - 1] !== "/") {
            negotiateUrl += "/";
        }
        negotiateUrl += "negotiate";
        negotiateUrl += index === -1 ? "" : url.substring(index);
        return negotiateUrl;
    };
    return HttpConnection;
}());
function transportMatches(requestedTransport, actualTransport) {
    return !requestedTransport || ((actualTransport & requestedTransport) !== 0);
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var JSON_HUB_PROTOCOL_NAME = "json";
/** Implements the JSON Hub Protocol. */
var JsonHubProtocol = /** @class */ (function () {
    function JsonHubProtocol() {
        /** @inheritDoc */
        this.name = JSON_HUB_PROTOCOL_NAME;
        /** @inheritDoc */
        this.version = 1;
        /** @inheritDoc */
        this.transferFormat = TransferFormat.Text;
    }
    /** Creates an array of {@link HubMessage} objects from the specified serialized representation.
     *
     * @param {string} input A string containing the serialized representation.
     * @param {ILogger} logger A logger that will be used to log messages that occur during parsing.
     */
    JsonHubProtocol.prototype.parseMessages = function (input, logger) {
        // The interface does allow "ArrayBuffer" to be passed in, but this implementation does not. So let's throw a useful error.
        if (typeof input !== "string") {
            throw new Error("Invalid input for JSON hub protocol. Expected a string.");
        }
        if (!input) {
            return [];
        }
        if (logger === null) {
            logger = NullLogger.instance;
        }
        // Parse the messages
        var messages = TextMessageFormat.parse(input);
        var hubMessages = [];
        for (var _i = 0, messages_1 = messages; _i < messages_1.length; _i++) {
            var message = messages_1[_i];
            var parsedMessage = JSON.parse(message);
            if (typeof parsedMessage.type !== "number") {
                throw new Error("Invalid payload.");
            }
            switch (parsedMessage.type) {
                case MessageType.Invocation:
                    this.isInvocationMessage(parsedMessage);
                    break;
                case MessageType.StreamItem:
                    this.isStreamItemMessage(parsedMessage);
                    break;
                case MessageType.Completion:
                    this.isCompletionMessage(parsedMessage);
                    break;
                case MessageType.Ping:
                    // Single value, no need to validate
                    break;
                case MessageType.Close:
                    // All optional values, no need to validate
                    break;
                default:
                    // Future protocol changes can add message types, old clients can ignore them
                    logger.log(LogLevel.Information, "Unknown message type '" + parsedMessage.type + "' ignored.");
                    continue;
            }
            hubMessages.push(parsedMessage);
        }
        return hubMessages;
    };
    /** Writes the specified {@link HubMessage} to a string and returns it.
     *
     * @param {HubMessage} message The message to write.
     * @returns {string} A string containing the serialized representation of the message.
     */
    JsonHubProtocol.prototype.writeMessage = function (message) {
        return TextMessageFormat.write(JSON.stringify(message));
    };
    JsonHubProtocol.prototype.isInvocationMessage = function (message) {
        this.assertNotEmptyString(message.target, "Invalid payload for Invocation message.");
        if (message.invocationId !== undefined) {
            this.assertNotEmptyString(message.invocationId, "Invalid payload for Invocation message.");
        }
    };
    JsonHubProtocol.prototype.isStreamItemMessage = function (message) {
        this.assertNotEmptyString(message.invocationId, "Invalid payload for StreamItem message.");
        if (message.item === undefined) {
            throw new Error("Invalid payload for StreamItem message.");
        }
    };
    JsonHubProtocol.prototype.isCompletionMessage = function (message) {
        if (message.result && message.error) {
            throw new Error("Invalid payload for Completion message.");
        }
        if (!message.result && message.error) {
            this.assertNotEmptyString(message.error, "Invalid payload for Completion message.");
        }
        this.assertNotEmptyString(message.invocationId, "Invalid payload for Completion message.");
    };
    JsonHubProtocol.prototype.assertNotEmptyString = function (value, errorMessage) {
        if (typeof value !== "string" || value === "") {
            throw new Error(errorMessage);
        }
    };
    return JsonHubProtocol;
}());

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
/** A builder for configuring {@link HubConnection} instances. */
var HubConnectionBuilder = /** @class */ (function () {
    function HubConnectionBuilder() {
    }
    HubConnectionBuilder.prototype.configureLogging = function (logging) {
        Arg.isRequired(logging, "logging");
        if (isLogger(logging)) {
            this.logger = logging;
        }
        else {
            this.logger = new ConsoleLogger(logging);
        }
        return this;
    };
    HubConnectionBuilder.prototype.withUrl = function (url, transportTypeOrOptions) {
        Arg.isRequired(url, "url");
        this.url = url;
        // Flow-typing knows where it's at. Since HttpTransportType is a number and IHttpConnectionOptions is guaranteed
        // to be an object, we know (as does TypeScript) this comparison is all we need to figure out which overload was called.
        if (typeof transportTypeOrOptions === "object") {
            this.httpConnectionOptions = transportTypeOrOptions;
        }
        else {
            this.httpConnectionOptions = {
                transport: transportTypeOrOptions,
            };
        }
        return this;
    };
    /** Configures the {@link HubConnection} to use the specified Hub Protocol.
     *
     * @param {IHubProtocol} protocol The {@link IHubProtocol} implementation to use.
     */
    HubConnectionBuilder.prototype.withHubProtocol = function (protocol) {
        Arg.isRequired(protocol, "protocol");
        this.protocol = protocol;
        return this;
    };
    /** Creates a {@link HubConnection} from the configuration options specified in this builder.
     *
     * @returns {HubConnection} The configured {@link HubConnection}.
     */
    HubConnectionBuilder.prototype.build = function () {
        // If httpConnectionOptions has a logger, use it. Otherwise, override it with the one
        // provided to configureLogger
        var httpConnectionOptions = this.httpConnectionOptions || {};
        // If it's 'null', the user **explicitly** asked for null, don't mess with it.
        if (httpConnectionOptions.logger === undefined) {
            // If our logger is undefined or null, that's OK, the HttpConnection constructor will handle it.
            httpConnectionOptions.logger = this.logger;
        }
        // Now create the connection
        if (!this.url) {
            throw new Error("The 'HubConnectionBuilder.withUrl' method must be called before building the connection.");
        }
        var connection = new HttpConnection(this.url, httpConnectionOptions);
        return HubConnection.create(connection, this.logger || NullLogger.instance, this.protocol || new JsonHubProtocol());
    };
    return HubConnectionBuilder;
}());
function isLogger(logger) {
    return logger.log !== undefined;
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Version token that will be replaced by the prepack command
/** The version of the SignalR client. */
var VERSION = "1.1.0-preview1-34410";



var esm = Object.freeze({
	VERSION: VERSION,
	AbortError: AbortError,
	HttpError: HttpError,
	TimeoutError: TimeoutError,
	DefaultHttpClient: DefaultHttpClient,
	HttpClient: HttpClient,
	HttpResponse: HttpResponse,
	HubConnection: HubConnection,
	get HubConnectionState () { return HubConnectionState; },
	HubConnectionBuilder: HubConnectionBuilder,
	get MessageType () { return MessageType; },
	get LogLevel () { return LogLevel; },
	get HttpTransportType () { return HttpTransportType; },
	get TransferFormat () { return TransferFormat; },
	NullLogger: NullLogger,
	JsonHubProtocol: JsonHubProtocol
});

var BinaryMessageFormat_1 = createCommonjsModule(function (module, exports) {
Object.defineProperty(exports, "__esModule", { value: true });
// Not exported from index.
var BinaryMessageFormat = /** @class */ (function () {
    function BinaryMessageFormat() {
    }
    // The length prefix of binary messages is encoded as VarInt. Read the comment in
    // the BinaryMessageParser.TryParseMessage for details.
    BinaryMessageFormat.write = function (output) {
        // msgpack5 uses returns Buffer instead of Uint8Array on IE10 and some other browser
        //  in which case .byteLength does will be undefined
        var size = output.byteLength || output.length;
        var lenBuffer = [];
        do {
            var sizePart = size & 0x7f;
            size = size >> 7;
            if (size > 0) {
                sizePart |= 0x80;
            }
            lenBuffer.push(sizePart);
        } while (size > 0);
        // msgpack5 uses returns Buffer instead of Uint8Array on IE10 and some other browser
        //  in which case .byteLength does will be undefined
        size = output.byteLength || output.length;
        var buffer = new Uint8Array(lenBuffer.length + size);
        buffer.set(lenBuffer, 0);
        buffer.set(output, lenBuffer.length);
        return buffer.buffer;
    };
    BinaryMessageFormat.parse = function (input) {
        var result = [];
        var uint8Array = new Uint8Array(input);
        var maxLengthPrefixSize = 5;
        var numBitsToShift = [0, 7, 14, 21, 28];
        for (var offset = 0; offset < input.byteLength;) {
            var numBytes = 0;
            var size = 0;
            var byteRead = void 0;
            do {
                byteRead = uint8Array[offset + numBytes];
                size = size | ((byteRead & 0x7f) << (numBitsToShift[numBytes]));
                numBytes++;
            } while (numBytes < Math.min(maxLengthPrefixSize, input.byteLength - offset) && (byteRead & 0x80) !== 0);
            if ((byteRead & 0x80) !== 0 && numBytes < maxLengthPrefixSize) {
                throw new Error("Cannot read message size.");
            }
            if (numBytes === maxLengthPrefixSize && byteRead > 7) {
                throw new Error("Messages bigger than 2GB are not supported.");
            }
            if (uint8Array.byteLength >= (offset + numBytes + size)) {
                // IE does not support .slice() so use subarray
                result.push(uint8Array.slice
                    ? uint8Array.slice(offset + numBytes, offset + numBytes + size)
                    : uint8Array.subarray(offset + numBytes, offset + numBytes + size));
            }
            else {
                throw new Error("Incomplete message.");
            }
            offset = offset + numBytes + size;
        }
        return result;
    };
    return BinaryMessageFormat;
}());
exports.BinaryMessageFormat = BinaryMessageFormat;

});

unwrapExports(BinaryMessageFormat_1);
var BinaryMessageFormat_2 = BinaryMessageFormat_1.BinaryMessageFormat;

var MessagePackHubProtocol_1 = createCommonjsModule(function (module, exports) {
Object.defineProperty(exports, "__esModule", { value: true });




// TypeDoc's @inheritDoc and @link don't work across modules :(
/** Implements the MessagePack Hub Protocol */
var MessagePackHubProtocol = /** @class */ (function () {
    function MessagePackHubProtocol() {
        /** The name of the protocol. This is used by SignalR to resolve the protocol between the client and server. */
        this.name = "messagepack";
        /** The version of the protocol. */
        this.version = 1;
        /** The TransferFormat of the protocol. */
        this.transferFormat = esm.TransferFormat.Binary;
    }
    /** Creates an array of HubMessage objects from the specified serialized representation.
     *
     * @param {ArrayBuffer} input An ArrayBuffer containing the serialized representation.
     * @param {ILogger} logger A logger that will be used to log messages that occur during parsing.
     */
    MessagePackHubProtocol.prototype.parseMessages = function (input, logger) {
        var _this = this;
        // The interface does allow "string" to be passed in, but this implementation does not. So let's throw a useful error.
        if (!(input instanceof ArrayBuffer)) {
            throw new Error("Invalid input for MessagePack hub protocol. Expected an ArrayBuffer.");
        }
        if (logger === null) {
            logger = esm.NullLogger.instance;
        }
        return BinaryMessageFormat_1.BinaryMessageFormat.parse(input).map(function (m) { return _this.parseMessage(m, logger); });
    };
    /** Writes the specified HubMessage to an ArrayBuffer and returns it.
     *
     * @param {HubMessage} message The message to write.
     * @returns {ArrayBuffer} An ArrayBuffer containing the serialized representation of the message.
     */
    MessagePackHubProtocol.prototype.writeMessage = function (message) {
        switch (message.type) {
            case esm.MessageType.Invocation:
                return this.writeInvocation(message);
            case esm.MessageType.StreamInvocation:
                return this.writeStreamInvocation(message);
            case esm.MessageType.StreamItem:
            case esm.MessageType.Completion:
                throw new Error("Writing messages of type '" + message.type + "' is not supported.");
            default:
                throw new Error("Invalid message type.");
        }
    };
    MessagePackHubProtocol.prototype.parseMessage = function (input, logger) {
        if (input.length === 0) {
            throw new Error("Invalid payload.");
        }
        var msgpack = msgpack5();
        var properties = msgpack.decode(new buffer.Buffer(input));
        if (properties.length === 0 || !(properties instanceof Array)) {
            throw new Error("Invalid payload.");
        }
        var messageType = properties[0];
        switch (messageType) {
            case esm.MessageType.Invocation:
                return this.createInvocationMessage(this.readHeaders(properties), properties);
            case esm.MessageType.StreamItem:
                return this.createStreamItemMessage(this.readHeaders(properties), properties);
            case esm.MessageType.Completion:
                return this.createCompletionMessage(this.readHeaders(properties), properties);
            case esm.MessageType.Ping:
                return this.createPingMessage(properties);
            case esm.MessageType.Close:
                return this.createCloseMessage(properties);
            default:
                // Future protocol changes can add message types, old clients can ignore them
                logger.log(esm.LogLevel.Information, "Unknown message type '" + messageType + "' ignored.");
                return null;
        }
    };
    MessagePackHubProtocol.prototype.createCloseMessage = function (properties) {
        // check minimum length to allow protocol to add items to the end of objects in future releases
        if (properties.length < 2) {
            throw new Error("Invalid payload for Close message.");
        }
        return {
            // Close messages have no headers.
            error: properties[1],
            type: esm.MessageType.Close,
        };
    };
    MessagePackHubProtocol.prototype.createPingMessage = function (properties) {
        // check minimum length to allow protocol to add items to the end of objects in future releases
        if (properties.length < 1) {
            throw new Error("Invalid payload for Ping message.");
        }
        return {
            // Ping messages have no headers.
            type: esm.MessageType.Ping,
        };
    };
    MessagePackHubProtocol.prototype.createInvocationMessage = function (headers, properties) {
        // check minimum length to allow protocol to add items to the end of objects in future releases
        if (properties.length < 5) {
            throw new Error("Invalid payload for Invocation message.");
        }
        var invocationId = properties[2];
        if (invocationId) {
            return {
                arguments: properties[4],
                headers: headers,
                invocationId: invocationId,
                target: properties[3],
                type: esm.MessageType.Invocation,
            };
        }
        else {
            return {
                arguments: properties[4],
                headers: headers,
                target: properties[3],
                type: esm.MessageType.Invocation,
            };
        }
    };
    MessagePackHubProtocol.prototype.createStreamItemMessage = function (headers, properties) {
        // check minimum length to allow protocol to add items to the end of objects in future releases
        if (properties.length < 4) {
            throw new Error("Invalid payload for StreamItem message.");
        }
        return {
            headers: headers,
            invocationId: properties[2],
            item: properties[3],
            type: esm.MessageType.StreamItem,
        };
    };
    MessagePackHubProtocol.prototype.createCompletionMessage = function (headers, properties) {
        // check minimum length to allow protocol to add items to the end of objects in future releases
        if (properties.length < 4) {
            throw new Error("Invalid payload for Completion message.");
        }
        var errorResult = 1;
        var voidResult = 2;
        var nonVoidResult = 3;
        var resultKind = properties[3];
        if (resultKind !== voidResult && properties.length < 5) {
            throw new Error("Invalid payload for Completion message.");
        }
        var completionMessage = {
            error: null,
            headers: headers,
            invocationId: properties[2],
            result: null,
            type: esm.MessageType.Completion,
        };
        switch (resultKind) {
            case errorResult:
                completionMessage.error = properties[4];
                break;
            case nonVoidResult:
                completionMessage.result = properties[4];
                break;
        }
        return completionMessage;
    };
    MessagePackHubProtocol.prototype.writeInvocation = function (invocationMessage) {
        var msgpack = msgpack5();
        var payload = msgpack.encode([esm.MessageType.Invocation, invocationMessage.headers || {}, invocationMessage.invocationId || null,
            invocationMessage.target, invocationMessage.arguments]);
        return BinaryMessageFormat_1.BinaryMessageFormat.write(payload.slice());
    };
    MessagePackHubProtocol.prototype.writeStreamInvocation = function (streamInvocationMessage) {
        var msgpack = msgpack5();
        var payload = msgpack.encode([esm.MessageType.StreamInvocation, streamInvocationMessage.headers || {}, streamInvocationMessage.invocationId,
            streamInvocationMessage.target, streamInvocationMessage.arguments]);
        return BinaryMessageFormat_1.BinaryMessageFormat.write(payload.slice());
    };
    MessagePackHubProtocol.prototype.readHeaders = function (properties) {
        var headers = properties[1];
        if (typeof headers !== "object") {
            throw new Error("Invalid headers.");
        }
        return headers;
    };
    return MessagePackHubProtocol;
}());
exports.MessagePackHubProtocol = MessagePackHubProtocol;

});

unwrapExports(MessagePackHubProtocol_1);
var MessagePackHubProtocol_2 = MessagePackHubProtocol_1.MessagePackHubProtocol;

var cjs = createCommonjsModule(function (module, exports) {
Object.defineProperty(exports, "__esModule", { value: true });
// Version token that will be replaced by the prepack command
/** The version of the SignalR Message Pack protocol library. */
exports.VERSION = "0.0.0-DEV_BUILD";

exports.MessagePackHubProtocol = MessagePackHubProtocol_1.MessagePackHubProtocol;

});

unwrapExports(cjs);
var cjs_1 = cjs.VERSION;
var cjs_2 = cjs.MessagePackHubProtocol;

var browserIndex = createCommonjsModule(function (module, exports) {
Object.defineProperty(exports, "__esModule", { value: true });

// This is where we add any polyfills we'll need for the browser. It is the entry module for browser-specific builds.
tslib_es6.__exportStar(cjs, exports);

});

var browserIndex$1 = unwrapExports(browserIndex);

return browserIndex$1;

})));
//# sourceMappingURL=signalr-protocol-msgpack.js.map

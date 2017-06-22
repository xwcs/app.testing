﻿#target "indesign"
#targetengine "session_CsBridge"

// this will help mantain C# <-> IND compatibility
var CsBridge_version = '0.0.2';

var CsBridge = (function(ind){
    // private closure
    var _indesign = ind;

    /// @Socket
    var _tube = null;

    // options
    var _opt = {
        url : 'localhost:13000'
    };

    /*
        map of elements
        {
            sender : <sender>
            kind: <event kind>
        }
    */
    var _handlers = new Object();


    // public interface
    var ret = {
        open : function(options){

           //_indesign.doScript(/*$.evalFile(*/new File("M:/Laco/Documents/Visual Studio 2015/Projects/ExcelProva/WindowsFormsApplication2/json2_src.js"));
           //$.evalFile(new File("M:/Laco/Documents/Visual Studio 2015/Projects/ExcelProva/WindowsFormsApplication2/xprototype.js"));

            __merge_options (options);
            __connect (_opt.url);
        },
        
        addEventHandler : function(evtSource, evtKind){
            evtSource.addEventListener(evtKind, __eventHandler);
        },

        close : function(){
            _tube.close();
        }    
    }

    var __eventHandler = function(evt){
        var ret = __sendMessage("JsEvent", {
            bubbles : evt.bubbles,
            cancelable : evt.cancelable,
            defaultPrevented: evt.defaultPrevented,
            eventType: evt.eventType,
            id: evt.id,
            index: evt.index,
            isValid: evt.isValid,
            propagationStopped: evt.propagationStopped,
            timeStamp: evt.timeStamp,
            currentTargetID: evt.currentTarget.id,
            parentID: evt.parent.id,
            targetID: evt.target.id
        });    
        
       __logResult(ret);
    
    }

    var __logResult = function (result){
        if(result.status == 'ok'){
                alert(JSON.stringify(result.data));
        }else{
                alert('Error:' + result.msg);
        }
    }

    var __getMessageKind = function(kindStr){
        switch(kindStr){
            case 'JsEvent' : return 1;
            default : return 0;
        }
    }

    var __sendMessage = function(kindStr, what){
        
        what.DataKindType = __getMessageKind(kindStr);

        _tube.write(JSON.stringify({
                id: 1,
                data: what
        }));
        
        // parse
        var result = _tube.read();
        if(result == "") return {status: 'error', msg: 'empty resposne'};
        try{
            return {status: 'ok', data : JSON.parse(result)};
        }catch(e){
            return {status: 'error', msg: e};
        }
    }
    
    // private functions
    var __connected = function(){
        return _tube != null && _tube.connected;
    }

    var __connect = function(url){
        if(__connected()) return;
        _tube = new Socket;
        if (_tube.open(url)) {
            return true;
        }
        // not good
        alert("Cant connect to C# server!");
        _tube = null;
        return false;
    }

    var __merge_options = function(opt){
        for(v in opt){
            _opt[v] = opt[v];
        }
    }

    return ret;
})(app)
 
/*
var myEventHandler = function(ev)
{
    alert( "You did: " + ev.target.name + " at " + ev.timeStamp );

    var conn = new Socket;
    if( conn.open( '127.0.0.1:13000' ) ) {
        conn.write( 'GET /indesign-page/ HTTP/1.0' + "Host: 127.0.0.1\r\n" + "\n\n" );
        reply = conn.read();// 999999);
        conn.close();
        alert(reply);
    } else {
        alert( 'Problem connecting to server' );
    }
};

function addEventHandler(kind, obj){
    alert(kind);
    obj.addEventListener(kind, myEventHandler);
} 



*/


/*
    json2.js
    2011-10-19

    Public Domain.

    NO WARRANTY EXPRESSED OR IMPLIED. USE AT YOUR OWN RISK.

    See http://www.JSON.org/js.html


    This code should be minified before deployment.
    See http://javascript.crockford.com/jsmin.html

    USE YOUR OWN COPY. IT IS EXTREMELY UNWISE TO LOAD CODE FROM SERVERS YOU DO
    NOT CONTROL.


    This file creates a global JSON object containing two methods: stringify
    and parse.

        JSON.stringify(value, replacer, space)
            value       any JavaScript value, usually an object or array.

            replacer    an optional parameter that determines how object
                        values are stringified for objects. It can be a
                        function or an array of strings.

            space       an optional parameter that specifies the indentation
                        of nested structures. If it is omitted, the text will
                        be packed without extra whitespace. If it is a number,
                        it will specify the number of spaces to indent at each
                        level. If it is a string (such as '\t' or '&nbsp;'),
                        it contains the characters used to indent at each level.

            This method produces a JSON text from a JavaScript value.

            When an object value is found, if the object contains a toJSON
            method, its toJSON method will be called and the result will be
            stringified. A toJSON method does not serialize: it returns the
            value represented by the name/value pair that should be serialized,
            or undefined if nothing should be serialized. The toJSON method
            will be passed the key associated with the value, and this will be
            bound to the value

            For example, this would serialize Dates as ISO strings.

                Date.prototype.toJSON = function (key) {
                    function f(n) {
                        // Format integers to have at least two digits.
                        return n < 10 ? '0' + n : n;
                    }

                    return this.getUTCFullYear()   + '-' +
                         f(this.getUTCMonth() + 1) + '-' +
                         f(this.getUTCDate())      + 'T' +
                         f(this.getUTCHours())     + ':' +
                         f(this.getUTCMinutes())   + ':' +
                         f(this.getUTCSeconds())   + 'Z';
                };

            You can provide an optional replacer method. It will be passed the
            key and value of each member, with this bound to the containing
            object. The value that is returned from your method will be
            serialized. If your method returns undefined, then the member will
            be excluded from the serialization.

            If the replacer parameter is an array of strings, then it will be
            used to select the members to be serialized. It filters the results
            such that only members with keys listed in the replacer array are
            stringified.

            Values that do not have JSON representations, such as undefined or
            functions, will not be serialized. Such values in objects will be
            dropped; in arrays they will be replaced with null. You can use
            a replacer function to replace those with JSON values.
            JSON.stringify(undefined) returns undefined.

            The optional space parameter produces a stringification of the
            value that is filled with line breaks and indentation to make it
            easier to read.

            If the space parameter is a non-empty string, then that string will
            be used for indentation. If the space parameter is a number, then
            the indentation will be that many spaces.

            Example:

            text = JSON.stringify(['e', {pluribus: 'unum'}]);
            // text is '["e",{"pluribus":"unum"}]'


            text = JSON.stringify(['e', {pluribus: 'unum'}], null, '\t');
            // text is '[\n\t"e",\n\t{\n\t\t"pluribus": "unum"\n\t}\n]'

            text = JSON.stringify([new Date()], function (key, value) {
                return this[key] instanceof Date ?
                    'Date(' + this[key] + ')' : value;
            });
            // text is '["Date(---current time---)"]'


        JSON.parse(text, reviver)
            This method parses a JSON text to produce an object or array.
            It can throw a SyntaxError exception.

            The optional reviver parameter is a function that can filter and
            transform the results. It receives each of the keys and values,
            and its return value is used instead of the original value.
            If it returns what it received, then the structure is not modified.
            If it returns undefined then the member is deleted.

            Example:

            // Parse the text. Values that look like ISO date strings will
            // be converted to Date objects.

            myData = JSON.parse(text, function (key, value) {
                var a;
                if (typeof value === 'string') {
                    a =
/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
                    if (a) {
                        return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4],
                            +a[5], +a[6]));
                    }
                }
                return value;
            });

            myData = JSON.parse('["Date(09/09/2001)"]', function (key, value) {
                var d;
                if (typeof value === 'string' &&
                        value.slice(0, 5) === 'Date(' &&
                        value.slice(-1) === ')') {
                    d = new Date(value.slice(5, -1));
                    if (d) {
                        return d;
                    }
                }
                return value;
            });


    This is a reference implementation. You are free to copy, modify, or
    redistribute.
*/

/*jslint evil: true, regexp: true */

/*members "", "\b", "\t", "\n", "\f", "\r", "\"", JSON, "\\", apply,
    call, charCodeAt, getUTCDate, getUTCFullYear, getUTCHours,
    getUTCMinutes, getUTCMonth, getUTCSeconds, hasOwnProperty, join,
    lastIndex, length, parse, prototype, push, replace, slice, stringify,
    test, toJSON, toString, valueOf
*/


// Create a JSON object only if one does not already exist. We create the
// methods in a closure to avoid creating global variables.

var JSON;
if (!JSON) {
    JSON = {};
}

(function () {
    'use strict';

    function f(n) {
        // Format integers to have at least two digits.
        return n < 10 ? '0' + n : n;
    }

    if (typeof Date.prototype.toJSON !== 'function') {

        Date.prototype.toJSON = function (key) {

            return isFinite(this.valueOf())
                ? this.getUTCFullYear()     + '-' +
                    f(this.getUTCMonth() + 1) + '-' +
                    f(this.getUTCDate())      + 'T' +
                    f(this.getUTCHours())     + ':' +
                    f(this.getUTCMinutes())   + ':' +
                    f(this.getUTCSeconds())   + 'Z'
                : null;
        };

        String.prototype.toJSON      =
            Number.prototype.toJSON  =
            Boolean.prototype.toJSON = function (key) {
                return this.valueOf();
            };
    }

    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        gap,
        indent,
        meta = {    // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"' : '\\"',
            '\\': '\\\\'
        },
        rep;


    function quote(string) {

        // If the string contains no control characters, no quote characters, and no
        // backslash characters, then we can safely slap some quotes around it.
        // Otherwise we must also replace the offending characters with safe escape
        // sequences.

        escapable.lastIndex = 0;
        return escapable.test(string) ? '"' + string.replace(escapable, function (a) {
            var c = meta[a];
            return typeof c === 'string'
                ? c
                : '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
        }) + '"' : '"' + string + '"';
    }


    function str(key, holder) {

        // Produce a string from holder[key].

        var i,          // The loop counter.
            k,          // The member key.
            v,          // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];

        // If the value has a toJSON method, call it to obtain a replacement value.

        if (value && typeof value === 'object' &&
                typeof value.toJSON === 'function') {
            value = value.toJSON(key);
        }

        // If we were called with a replacer function, then call the replacer to
        // obtain a replacement value.

        if (typeof rep === 'function') {
            value = rep.call(holder, key, value);
        }

        // What happens next depends on the value's type.

        switch (typeof value) {
            case 'string':
                return quote(value);

            case 'number':

                // JSON numbers must be finite. Encode non-finite numbers as null.

                return isFinite(value) ? String(value) : 'null';

            case 'boolean':
            case 'null':

                // If the value is a boolean or null, convert it to a string. Note:
                // typeof null does not produce 'null'. The case is included here in
                // the remote chance that this gets fixed someday.

                return String(value);

                // If the type is 'object', we might be dealing with an object or an array or
                // null.

            case 'object':

                // Due to a specification blunder in ECMAScript, typeof null is 'object',
                // so watch out for that case.

                if (!value) {
                    return 'null';
                }

                // Make an array to hold the partial results of stringifying this object value.

                gap += indent;
                partial = [];

                // Is the value an array?

                if (Object.prototype.toString.apply(value) === '[object Array]') {

                    // The value is an array. Stringify every element. Use null as a placeholder
                    // for non-JSON values.

                    length = value.length;
                    for (i = 0; i < length; i += 1) {
                        partial[i] = str(i, value) || 'null';
                    }

                    // Join all of the elements together, separated with commas, and wrap them in
                    // brackets.

                    v = partial.length === 0
                        ? '[]'
                        : gap
                        ? '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']'
                        : '[' + partial.join(',') + ']';
                    gap = mind;
                    return v;
                }

                // If the replacer is an array, use it to select the members to be stringified.

                if (rep && typeof rep === 'object') {
                    length = rep.length;
                    for (i = 0; i < length; i += 1) {
                        if (typeof rep[i] === 'string') {
                            k = rep[i];
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                } else {

                    // Otherwise, iterate through all of the keys in the object.

                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                }

                // Join all of the member texts together, separated with commas,
                // and wrap them in braces.

                v = partial.length === 0
                    ? '{}'
                    : gap
                    ? '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}'
                    : '{' + partial.join(',') + '}';
                gap = mind;
                return v;
        }
    }

    // If the JSON object does not yet have a stringify method, give it one.

    if (typeof JSON.stringify !== 'function') {
        JSON.stringify = function (value, replacer, space) {

            // The stringify method takes a value and an optional replacer, and an optional
            // space parameter, and returns a JSON text. The replacer can be a function
            // that can replace values, or an array of strings that will select the keys.
            // A default replacer method can be provided. Use of the space parameter can
            // produce text that is more easily readable.

            var i;
            gap = '';
            indent = '';

            // If the space parameter is a number, make an indent string containing that
            // many spaces.

            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }

                // If the space parameter is a string, it will be used as the indent string.

            } else if (typeof space === 'string') {
                indent = space;
            }

            // If there is a replacer, it must be a function or an array.
            // Otherwise, throw an error.

            rep = replacer;
            if (replacer && typeof replacer !== 'function' &&
                    (typeof replacer !== 'object' ||
                    typeof replacer.length !== 'number')) {
                throw new Error('JSON.stringify');
            }

            // Make a fake root object containing our value under the key of ''.
            // Return the result of stringifying the value.

            return str('', {'': value});
        };
    }


    // If the JSON object does not yet have a parse method, give it one.

    if (typeof JSON.parse !== 'function') {
        JSON.parse = function (text, reviver) {

            // The parse method takes a text and an optional reviver function, and returns
            // a JavaScript value if the text is a valid JSON text.

            var j;

            function walk(holder, key) {

                // The walk method is used to recursively walk the resulting structure so
                // that modifications can be made.

                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v;
                            } else {
                                delete value[k];
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value);
            }


            // Parsing happens in four stages. In the first stage, we replace certain
            // Unicode characters with escape sequences. JavaScript handles many characters
            // incorrectly, either silently deleting them, or treating them as line endings.

            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx, function (a) {
                    return '\\u' +
                        ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
                });
            }

            // In the second stage, we run the text against regular expressions that look
            // for non-JSON patterns. We are especially concerned with '()' and 'new'
            // because they can cause invocation, and '=' because it can cause mutation.
            // But just to be safe, we want to reject all unexpected forms.

            // We split the second stage into 4 regexp operations in order to work around
            // crippling inefficiencies in IE's and Safari's regexp engines. First we
            // replace the JSON backslash pairs with '@' (a non-JSON character). Second, we
            // replace all simple value tokens with ']' characters. Third, we delete all
            // open brackets that follow a colon or comma or that begin the text. Finally,
            // we look to see that the remaining characters are only whitespace or ']' or
            // ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

            if (/^[\],:{}\s]*$/
                    .test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@')
                        .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
                        .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

                // In the third stage we use the eval function to compile the text into a
                // JavaScript structure. The '{' operator is subject to a syntactic ambiguity
                // in JavaScript: it can begin a block or an object literal. We wrap the text
                // in parens to eliminate the ambiguity.

                j = eval('(' + text + ')');

                // In the optional fourth stage, we recursively walk the new structure, passing
                // each name/value pair to a reviver function for possible transformation.

                return typeof reviver === 'function'
                    ? walk({'': j}, '')
                    : j;
            }

            // If the text is not JSON parseable, then a SyntaxError is thrown.

            throw new SyntaxError('JSON.parse');
        };
    }
}());
/*
$Log: json2_src.js,v $
Revision 1.1  2013/10/02 13:30:11  lsopko-3di
release of candidate

*/

(function(global) {
    function isArray(arr) {
        return Object.prototype.toString.call(arr) === '[object Array]';
    }

    function foreach(arr, handler) {
        if (isArray(arr)) {
            for (var i = 0; i < arr.length; i++) {
                handler(arr[i]);
            }
        }
        else
            handler(arr);
    }

    function D(fn) {
        var status = 'pending',
			doneFuncs = [],
			failFuncs = [],
			progressFuncs = [],
			resultArgs = null,

		promise = {
		    done: function() {
		        for (var i = 0; i < arguments.length; i++) {
		            // skip any undefined or null arguments
		            if (!arguments[i]) {
		                continue;
		            }

		            if (isArray(arguments[i])) {
		                var arr = arguments[i];
		                for (var j = 0; j < arr.length; j++) {
		                    // immediately call the function if the deferred has been resolved
		                    if (status === 'resolved') {
		                        arr[j].apply(this, resultArgs);
		                    }

		                    doneFuncs.push(arr[j]);
		                }
		            }
		            else {
		                // immediately call the function if the deferred has been resolved
		                if (status === 'resolved') {
		                    arguments[i].apply(this, resultArgs);
		                }

		                doneFuncs.push(arguments[i]);
		            }
		        }
				
		        return this;
		    },

		    fail: function() {
		        for (var i = 0; i < arguments.length; i++) {
		            // skip any undefined or null arguments
		            if (!arguments[i]) {
		                continue;
		            }

		            if (isArray(arguments[i])) {
		                var arr = arguments[i];
		                for (var j = 0; j < arr.length; j++) {
		                    // immediately call the function if the deferred has been resolved
		                    if (status === 'rejected') {
		                        arr[j].apply(this, resultArgs);
		                    }

		                    failFuncs.push(arr[j]);
		                }
		            }
		            else {
		                // immediately call the function if the deferred has been resolved
		                if (status === 'rejected') {
		                    arguments[i].apply(this, resultArgs);
		                }

		                failFuncs.push(arguments[i]);
		            }
		        }
				
		        return this;
		    },

		    always: function() {
		        return this.done.apply(this, arguments).fail.apply(this, arguments);
		    },

		    progress: function() {
		        for (var i = 0; i < arguments.length; i++) {
		            // skip any undefined or null arguments
		            if (!arguments[i]) {
		                continue;
		            }

		            if (isArray(arguments[i])) {
		                var arr = arguments[i];
		                for (var j = 0; j < arr.length; j++) {
		                    // immediately call the function if the deferred has been resolved
		                    if (status === 'pending') {
		                        progressFuncs.push(arr[j]);
		                    }
		                }
		            }
		            else {
		                // immediately call the function if the deferred has been resolved
		                if (status === 'pending') {
		                    progressFuncs.push(arguments[i]);
		                }
		            }
		        }
				
		        return this;
		    },

		    then: function() {
		        // fail callbacks
		        if (arguments.length > 1 && arguments[1]) {
		            this.fail(arguments[1]);
		        }

		        // done callbacks
		        if (arguments.length > 0 && arguments[0]) {
		            this.done(arguments[0]);
		        }

		        // notify callbacks
		        if (arguments.length > 2 && arguments[2]) {
		            this.progress(arguments[2]);
		        }
		    },

		    promise: function(obj) {
		        if (obj == null) {
		            return promise;
		        } else {
		            for (var i in promise) {
		                obj[i] = promise[i];
		            }
		            return obj;
		        }
		    },

		    state: function() {
		        return status;
		    },

		    debug: function() {
		        console.log('[debug]', doneFuncs, failFuncs, status);
		    },

		    isRejected: function() {
		        return status === 'rejected';
		    },

		    isResolved: function() {
		        return status === 'resolved';
		    },

		    pipe: function(done, fail, progress) {
		        return D(function(def) {
		            foreach(done, function(func) {
		                // filter function
		                if (typeof func === 'function') {
		                    deferred.done(function() {
		                        var returnval = func.apply(this, arguments);
		                        // if a new deferred/promise is returned, its state is passed to the current deferred/promise
		                        if (returnval && typeof returnval === 'function') {
		                            returnval.promise().then(def.resolve, def.reject, def.notify);
		                        }
		                        else {	// if new return val is passed, it is passed to the piped done
		                            def.resolve(returnval);
		                        }
		                    });
		                }
		                else {
		                    deferred.done(def.resolve);
		                }
		            });

		            foreach(fail, function(func) {
		                if (typeof func === 'function') {
		                    deferred.fail(function() {
		                        var returnval = func.apply(this, arguments);
								
		                        if (returnval && typeof returnval === 'function') {
		                            returnval.promise().then(def.resolve, def.reject, def.notify);
		                        } else {
		                            def.reject(returnval);
		                        }
		                    });
		                }
		                else {
		                    deferred.fail(def.reject);
		                }
		            });
		        }).promise();
		    }
		},

		deferred = {
		    resolveWith: function(context) {
		        if (status === 'pending') {
		            status = 'resolved';
		            var args = resultArgs = (arguments.length > 1) ? arguments[1] : [];
		            for (var i = 0; i < doneFuncs.length; i++) {
		                doneFuncs[i].apply(context, args);
		            }
		        }
		        return this;
		    },

		    rejectWith: function(context) {
		        if (status === 'pending') {
		            status = 'rejected';
		            var args = resultArgs = (arguments.length > 1) ? arguments[1] : [];
		            for (var i = 0; i < failFuncs.length; i++) {
		                failFuncs[i].apply(context, args);
		            }
		        }
		        return this;
		    },

		    notifyWith: function(context) {
		        if (status === 'pending') {
		            var args = resultArgs = (arguments.length > 1) ? arguments[1] : [];
		            for (var i = 0; i < progressFuncs.length; i++) {
		                progressFuncs[i].apply(context, args);
		            }
		        }
		        return this;
		    },

		    resolve: function() {
		        return this.resolveWith(this, arguments);
		    },

		    reject: function() {
		        return this.rejectWith(this, arguments);
		    },

		    notify: function() {
		        return this.notifyWith(this, arguments);
		    }
		}

        var obj = promise.promise(deferred);

        if (fn) {
            fn.apply(obj, [obj]);
        }

        return obj;
    }

    D.when = function() {
        if (arguments.length < 2) {
            var obj = arguments.length ? arguments[0] : undefined;
            if (obj && (typeof obj.isResolved === 'function' && typeof obj.isRejected === 'function')) {
                return obj.promise();			
            }
            else {
                return D().resolve(obj).promise();
            }
        }
        else {
            return (function(args){
                var df = D(),
					size = args.length,
					done = 0,
					rp = new Array(size);	// resolve params: params of each resolve, we need to track down them to be able to pass them in the correct order if the master needs to be resolved

                for (var i = 0; i < args.length; i++) {
                    (function(j) {
                        var obj = null;
                        
                        if (args[j].done) {
                            args[j].done(function() { rp[j] = (arguments.length < 2) ? arguments[0] : arguments; if (++done == size) { df.resolve.apply(df, rp); }})
                            .fail(function() { df.reject(arguments); });
                        } else {
                            obj = args[j];
                            args[j] = new Deferred();
                            
                            args[j].done(function() { rp[j] = (arguments.length < 2) ? arguments[0] : arguments; if (++done == size) { df.resolve.apply(df, rp); }})
                            .fail(function() { df.reject(arguments); }).resolve(obj);
                        }
                    })(i);
                }

                return df.promise();
            })(arguments);
        }
    }

    global.Deferred = D;
})(CsBridge);
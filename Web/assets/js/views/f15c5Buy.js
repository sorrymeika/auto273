﻿define('views/f15c5Buy',['zepto','ui/sl','util','app','views/loading','views/buy'],function (require,exports,module) {
    var $=require('zepto'),
        sl=require('ui/sl'),
        app=require('app'),
        util=require('util'),
        Loading=require('views/loading'),
        Buy=require('views/buy');

    module.exports=Buy.extend({
        GameID: '90015',
        title: '15选5投注',
        BetDataKey: 'f15c5BetData',
        backUrl: '/f15c5.html'
    });
});

define(['zepto','sl/sl','app','sl/widget/dropdown'],function (require,exports,module) {
    var $=require('zepto'),
        sl=require('sl/sl'),
        Dropdown=require('sl/widget/dropdown'),
        app=require('app');

    module.exports=sl.Activity.extend({
        template: 'views/add.html',
        events: {},
        onCreate: function () {
            var that=this;

            that.dropdown=new Dropdown({
                data: [{
                    text: '过户',
                    value: 0
                },{
                    text: '转籍',
                    value: 1
                }],
                isFixed: true,
                attacher: that.$('.js_dropdown'),
                onChange: function (e,i,dataItem) {
                    that.$('.js_dropdown').html(dataItem.text);
                    (dataItem.value==1)?that.$('.js_region').show():that.$('.js_region').hide()
                }
            })
        },
        onStart: function () {
        },
        onResume: function () {
            console.log("index onResume");
        },
        onShow: function () {
            if(!localStorage.getItem('USERINFO')) {
                this.redirect('/login.html');
            }
        },
        onDestory: function () {
            console.log("index onDestory");
        }

    });
});

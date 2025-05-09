module App.Styles

open Sutil
open Sutil.Styling

let geoapify() =
    let style =
        [
            rule ".autocomplete-container" [
                Css.positionRelative
                
            ]
            rule ".geoapify-autocomplete-input" [
                Css.displayFlex
                //Css.marginBottom 20
            ]
            rule ".geoapify-close-button visible" [
                //Css.marginTop 20
            ]
            rule ".geoapify-autocomplete-input input" [
                Css.flex 1
                Css.outlineStyleNone
                Css.custom ("border","1px solid rgba(0, 0, 0, 0.2)")
                Css.padding 10
                Css.paddingRight 31
                Css.fontSize 16
            ]

        ]
    style |> addGlobalStyleSheet (Browser.Dom.document) //""